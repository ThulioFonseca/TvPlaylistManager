using System.IO.Compression;
using TvPlaylistManager.Application.Contracts.Dtos;
using TvPlaylistManager.Application.Contracts.Exceptions;
using TvPlaylistManager.Domain.Interfaces;
using TvPlaylistManager.Domain.Models.Epg;
using TvPlaylistManager.Infrastructure.Extensions;

namespace TvPlaylistManager.Domain.Services.Epg
{
    public class EpgService : IEpgService
    {
        private readonly ILogger<EpgService> _logger;
        private readonly IEpgRepository _epgRepository;
        private readonly HttpClient _httpClient;
        private readonly INotificationHandler _notificationHandler;

        public EpgService(ILogger<EpgService> logger, IEpgRepository epgRepository, IHttpClientFactory httpClientFactory, INotificationHandler notificationHandler)
        {
            _logger = logger;
            _epgRepository = epgRepository;
            _httpClient = httpClientFactory.CreateClient("EpgClient");
            _notificationHandler = notificationHandler;
        }

        public async Task DeleteEpgSource(long id)
        {
            try
            {
                await _epgRepository.DeleteAsync(id);
                await _notificationHandler.Handle(new() { Type = Enums.NotificationType.Success, Message = "Epg source Deleted!" });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{EpgService} - Unexpected error when deleting EPG", nameof(EpgService));
                await _notificationHandler.Handle(new() { Type = Enums.NotificationType.Error, Message = ex.Message });

            }
        }

        public async Task<IEnumerable<EpgSource>> GetAllEpgSources()
        {
            var sources = await _epgRepository.GetAllAsync();
            return sources;
        }

        public async Task<EpgSource?> GetEpgSourceById(long id)
        {
            return await _epgRepository.GetByIdAsync(id, x => x.Channels);
        }

        public async Task SaveEpgSource(EpgSource epgSource)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(epgSource);

                var epgChannels = await GetEpgChannels(epgSource);

                epgSource.Channels = epgChannels;   

                await _epgRepository.AddAsync(epgSource);

                _logger.LogInformation("{EpgService} - EpgSource saved successfully", nameof(EpgService));
                await _notificationHandler.Handle(new() { Type = Enums.NotificationType.Success, Message = "Epg source saved!" });


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{EpgService} - Unexpected Error", nameof(EpgService));
                await _notificationHandler.Handle(new() { Type = Enums.NotificationType.Error, Message = ex.Message });
            }
        }

        private async Task<List<EpgChannel>> GetEpgChannels(EpgSource epgSource)
        {
            var channels = new List<EpgChannel>();

            if (!string.IsNullOrEmpty(epgSource.Url))
            {
                var response = await _httpClient.GetAsync(epgSource.Url);

                if (response.IsSuccessStatusCode)
                {
                    await using var content = await response.Content.ReadAsStreamAsync();

                    bool isGzip = response.Content.Headers.ContentType?.MediaType == "application/gzip" || IsGzipStream(content);

                    await using var finalStream = isGzip ? new GZipStream(content, CompressionMode.Decompress) : content;

                    if (XmlHelper.DeserializeFromStream<EpgXmlDto>(finalStream) is EpgXmlDto result)
                    {
                        channels = [.. result.Channels.Select(x => new EpgChannel()
                        {
                            ChannelEpgId = x.Id,
                            Name = x.DisplayName,
                            EpgSourceId = epgSource.Id,
                            EpgSource = epgSource,
                            IconUrl = x.Icons.FirstOrDefault()?.IconUrl,
                        })];
                    }
                }
                else
                {
                    _logger.LogWarning("{EpgService} - Unsuccessful request: Status code - {StatusCode}, Respoonse - {Content} ", nameof(EpgService), response.StatusCode, response.Content);
                    throw new BusinessException(string.Format("{0} - Unsuccessful request: Status code - {1}, Respoonse - {2} ", nameof(EpgService), response.StatusCode, response.Content));
                }
            }

            return channels;
        }

        public async Task UpdateEpgSoure(EpgSource epgSource)
        {
            try
            {
                await _epgRepository.UpdateAsync(epgSource);
                await _notificationHandler.Handle(new() { Type = Enums.NotificationType.Success, Message = "Epg source updated!"});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{EpgService} - Unexpected error when updating EPG", nameof(EpgService));
                await _notificationHandler.Handle(new() { Type = Enums.NotificationType.Error, Message = ex.Message});
            }
        }

        private static bool IsGzipStream(Stream stream)
        {
            const byte gzipMagic1 = 0x1f;
            const byte gzipMagic2 = 0x8b;

            stream.Seek(0, SeekOrigin.Begin);
            int firstByte = stream.ReadByte();
            int secondByte = stream.ReadByte();
            stream.Seek(0, SeekOrigin.Begin);

            return firstByte == gzipMagic1 && secondByte == gzipMagic2;
        }
    }
}
