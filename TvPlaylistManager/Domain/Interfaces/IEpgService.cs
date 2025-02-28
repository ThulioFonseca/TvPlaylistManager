using TvPlaylistManager.Domain.Models.Epg;

namespace TvPlaylistManager.Domain.Interfaces
{
    public interface IEpgService
    {
        Task SaveEpgSource(EpgSource epgSource);
        Task<IEnumerable<EpgSource>> GetAllEpgSources();
        Task<EpgSource?> GetEpgSourceById(long id);
        Task<EpgSource?> UpdateEpgSoure(EpgSource epgSource);
        Task DeleteEpgSource(long id);
    }
}
