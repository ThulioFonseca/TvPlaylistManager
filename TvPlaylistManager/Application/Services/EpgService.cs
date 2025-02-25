using System.Xml.Serialization;
using TvPlaylistManager.Application.Contracts.Dtos;
using TvPlaylistManager.Application.Contracts.Interfaces;
using TvPlaylistManager.Domain.Models.Epg;

namespace TvPlaylistManager.Application.Services
{
    public class EpgService : IEpgService
    {
        private readonly ILogger<EpgService> _logger;
        private readonly IEpgRepository _epgRepository;
        private readonly HttpClient _httpClient;

        public EpgService(ILogger<EpgService> logger, IEpgRepository epgRepository, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _epgRepository = epgRepository;
            _httpClient = httpClientFactory.CreateClient("EpgClient");
        }

        public async Task DeleteEpgSource(long id)
        {
            try
            {
                await _epgRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{EpgService} - Unexpected error when deleting EPG", nameof(EpgService));
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

        public async Task<EpgSource?> SaveEpgSource(EpgSource epgSource)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(epgSource);

                if (!string.IsNullOrEmpty(epgSource.Url))
                {
                    var response = await _httpClient.GetAsync(epgSource.Url);

                    if (response.IsSuccessStatusCode)
                    {
                        await using var content = await response.Content.ReadAsStreamAsync();

                        var serializer = new XmlSerializer(typeof(EpgXmlDto));

                        if (serializer.Deserialize(content) is EpgXmlDto resultado)
                        {
                            var channels = resultado.Channels.Select(x => new EpgChannel()
                            {
                                ChannelEpgId = x.Id,
                                Name = x.DisplayName,
                                EpgSourceId = epgSource.Id,
                                EpgSource = epgSource,
                                IconUrl = x.Icon,
                            }).ToList();

                            epgSource.Channels = channels;
                        }
                    }
                    else
                        _logger.LogWarning("{EpgService} -Unsuccessful request: Status code - {StatusCode}, Respoonse - {Content} ", nameof(EpgService), response.StatusCode, response.Content);

                }

                await _epgRepository.AddAsync(epgSource);

                _logger.LogInformation("{EpgService} - EpgSource saved successfully", nameof(EpgService));

                return epgSource;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "{EpgService} - Failed to execute the request", nameof(EpgService));
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{EpgService} - Unexpected Error", nameof(EpgService));
                return null;
            }
        }

        public async Task<EpgSource?> UpdateEpgSoure(EpgSource epgSource)
        {
            try
            {
                await _epgRepository.UpdateAsync(epgSource);
                return epgSource;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{EpgService} - Unexpected error when updating EPG", nameof(EpgService));
                return null;

            }
        }
    }
}
