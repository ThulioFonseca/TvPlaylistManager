using TvPlaylistManager.Domain.Models.Epg;

namespace TvPlaylistManager.Application.Contracts.Interfaces
{
    public interface IEpgService
    {
        Task<EpgSource?> SaveEpgSource(EpgSource epgSource);
        Task<IEnumerable<EpgSource>> GetAllEpgSources();
        Task<EpgSource?> GetEpgSourceById(long id);
        Task<EpgSource?> UpdateEpgSoure(EpgSource epgSource);
        Task DeleteEpgSource(long id);
    }
}
