using TvPlaylistManager.Application.Contracts.Exceptions;
using TvPlaylistManager.Domain.Enums;
using TvPlaylistManager.Domain.Interfaces;
using TvPlaylistManager.Domain.Models.M3u;
using TvPlaylistManager.Domain.Services.Epg;
using TvPlaylistManager.Infrastructure.Extensions;

namespace TvPlaylistManager.Domain.Services.M3U
{
    public class M3UService : IM3UService
    {
        private readonly IM3URepository _m3URepository;
        private readonly ILogger<M3UService> _logger;
        private readonly HttpClient _httpClient;
        private readonly INotificationHandler _notificationHandler;

        public M3UService(IM3URepository m3URepository, ILogger<M3UService> logger, IHttpClientFactory httpClientFactory, INotificationHandler notificationHandler)
        {
            _m3URepository = m3URepository;
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient("M3UClient");
            _notificationHandler = notificationHandler;
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
                    var content =  await response.Content.ReadAsStringAsync();

                    m3uPlaylist.ChannelGroups = content.Deserialize();
                }
                else
                {
                    _logger.LogWarning("{M3UService} - Unsuccessful request: Status code - {StatusCode}, Respoonse - {Content} ", nameof(M3UService), response.StatusCode, response.Content);
                    throw new BusinessException(string.Format("{0} - Unsuccessful request: Status code - {1}, Respoonse - {2} ", nameof(M3UService), response.StatusCode, response.Content));
                }
            }

        }

    }
}
