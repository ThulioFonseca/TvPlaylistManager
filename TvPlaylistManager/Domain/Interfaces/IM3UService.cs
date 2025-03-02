using TvPlaylistManager.Domain.Models.M3u;

namespace TvPlaylistManager.Domain.Interfaces
{
    public interface IM3UService
    {
        Task SaveM3uPlaylist(M3UPlaylist m3uPlaylist);
        Task<IEnumerable<M3UPlaylist>> GetAllM3uPlaylits();
        Task<M3UPlaylist> GetM3uPlaylistById(long id);
        Task UpdateM3uPlaylist(M3UPlaylist m3uPlaylist);
        Task DeleteM3uPlaylist(long id);
    }
}
