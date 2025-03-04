using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;
using TvPlaylistManager.Application.Contracts.Exceptions;
using TvPlaylistManager.Domain.Enums;
using TvPlaylistManager.Domain.Interfaces;
using TvPlaylistManager.Domain.Models.M3u;
using TvPlaylistManager.Domain.Services.Epg;
using TvPlaylistManager.Infrastructure.Extensions;
using TvPlaylistManager.Domain.Models.Epg;

namespace TvPlaylistManager.Domain.Services.M3U
{
    public partial class M3UService : IM3UService
    {
        private readonly IM3URepository _m3URepository;
        private readonly IEpgRepository _epgRepository;
        private readonly ILogger<M3UService> _logger;
        private readonly HttpClient _httpClient;
        private readonly INotificationHandler _notificationHandler;
        private const string _channelNamePatternA = @"[^a-zA-Z0-9]";
        private const string _channelNamePatternB = @"\s+";


        public M3UService(IM3URepository m3URepository, ILogger<M3UService> logger, IHttpClientFactory httpClientFactory, INotificationHandler notificationHandler, IEpgRepository epgRepository)
        {
            _m3URepository = m3URepository;
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient("M3UClient");
            _notificationHandler = notificationHandler;
            _epgRepository = epgRepository;
        }

        public async Task DeleteM3uPlaylist(long id)
        {
            try
            {
                await _m3URepository.DeleteAsync(id);
                await _notificationHandler.Handle(NotificationType.Success, "M3U Playlist Deleted!");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{M3UService} - Unexpected error when deleting M3U Playlist.", nameof(M3UService));
                await _notificationHandler.Handle(NotificationType.Error, ex.Message);

            }
        }

        public async Task<IEnumerable<M3UPlaylist>> GetAllM3uPlaylits()
        {
            var sources = await _m3URepository.GetAllM3uPlaylitsAsync();
            return sources;
        }

        public async Task<M3UPlaylist> GetM3uPlaylistById(long id)
        {
            return await _m3URepository.GetByIdAsync(id);
        }

        public async Task SaveM3uPlaylist(M3UPlaylist m3uPlaylist)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(m3uPlaylist);

                await GetPlaylistChannels(m3uPlaylist);

                await MatchChannelsEpg(m3uPlaylist);

                await _m3URepository.AddAsync(m3uPlaylist);

                _logger.LogInformation("{M3UService} - Playlist saved successfully", nameof(M3UService));
                await _notificationHandler.Handle(NotificationType.Success, "Playlist saved!");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{M3UService} - Unexpected Error", nameof(M3UService));
                await _notificationHandler.Handle(NotificationType.Error, ex.Message);
            }
        }

        public async Task UpdateM3uPlaylist(M3UPlaylist m3uPlaylist)
        {
            try
            {
                await MatchChannelsEpg(m3uPlaylist);
                await _m3URepository.UpdateAsync(m3uPlaylist);
                await _notificationHandler.Handle(NotificationType.Success, "M3U Playlist updated!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{M3UService} - Unexpected error when updating M3U Playlist", nameof(EpgService));
                await _notificationHandler.Handle(NotificationType.Error, ex.Message);
            }
        }

        private async Task GetPlaylistChannels(M3UPlaylist m3uPlaylist)
        {
            if (!string.IsNullOrEmpty(m3uPlaylist.Url))
            {
                var response = await _httpClient.GetAsync(m3uPlaylist.Url);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    m3uPlaylist.ChannelGroups = content.Deserialize(true);
                }
                else
                {
                    _logger.LogWarning("{M3UService} - Unsuccessful request: Status code - {StatusCode}, Respoonse - {Content} ", nameof(M3UService), response.StatusCode, response.Content);
                    throw new BusinessException(string.Format("{0} - Unsuccessful request: Status code - {1}, Respoonse - {2} ", nameof(M3UService), response.StatusCode, response.Content));
                }
            }

        }

        private static EpgChannel FindBestMatch(string channelName, EpgSource epgSource)
        {
            const double SimilarityThreshold = 0.6;

            if (string.IsNullOrWhiteSpace(channelName)) return null;

            string normalizedM3UName = NormalizeChannelName(channelName);
            EpgChannel bestMatch = null;
            int bestScore = int.MaxValue;
            int m3uLength = normalizedM3UName.Length;

            foreach (var epgChannel in epgSource.Channels)
            {
                string normalizedEpgName = NormalizeChannelName(epgChannel.Name);
                int score = LevenshteinDistance(normalizedM3UName, normalizedEpgName);

                double similarity = 1.0 - (double)score / Math.Max(m3uLength, normalizedEpgName.Length);

                if (similarity >= SimilarityThreshold && score < bestScore)
                {
                    bestScore = score;
                    bestMatch = epgChannel;
                }
            }

            return bestMatch;
        }

        private static int LevenshteinDistance(string source, string target)
        {
            if (string.IsNullOrEmpty(source)) return target.Length;
            if (string.IsNullOrEmpty(target)) return source.Length;

            int[,] matrix = new int[source.Length + 1, target.Length + 1];

            for (int i = 0; i <= source.Length; matrix[i, 0] = i++) { }
            for (int j = 0; j <= target.Length; matrix[0, j] = j++) { }

            for (int i = 1; i <= source.Length; i++)
            {
                for (int j = 1; j <= target.Length; j++)
                {
                    int cost = (source[i - 1] == target[j - 1]) ? 0 : 1;

                    matrix[i, j] = Math.Min(
                        Math.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1),
                        matrix[i - 1, j - 1] + cost);
                }
            }

            return matrix[source.Length, target.Length];
        }

        public async Task MatchChannelsEpg(M3UPlaylist playlist)
        {
            var epgSource = await _epgRepository.GetByIdAsync(playlist.EpgSourceId.Value, x => x.Channels);

            foreach (var group in playlist.ChannelGroups)
            {
                foreach (var channel in group.Channels)
                {
                    var bestMatch = FindBestMatch(channel.Name, epgSource);

                    if (bestMatch != null)
                    {
                        channel.TvgId = bestMatch.ChannelEpgId;
                        channel.TvgName = bestMatch.Name;
                        channel.TvgLogo = string.IsNullOrEmpty(bestMatch.IconUrl) ? channel.TvgLogo : bestMatch.IconUrl;
                    }
                    else
                    {
                        channel.TvgId = null;
                        channel.TvgName = null;
                    }
                }
            }

            playlist.UpdatedAt = DateTime.Now;
        }

        private static string NormalizeChannelName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return string.Empty;

            name = name.Normalize(NormalizationForm.FormD);
            name = new string(name.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray());

            name = ChannelNameRegexA().Replace(name, " ");
            name = ChannelNameRegexB().Replace(name, " ").Trim();

            return name.ToLower();
        }

        [GeneratedRegex(_channelNamePatternA)]
        private static partial Regex ChannelNameRegexA();

        [GeneratedRegex(_channelNamePatternB)]
        private static partial Regex ChannelNameRegexB();
    }
}
