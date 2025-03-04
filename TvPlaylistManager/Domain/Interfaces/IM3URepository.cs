using TvPlaylistManager.Domain.Models.M3u;

namespace TvPlaylistManager.Domain.Interfaces
{
    public interface IM3URepository : IRepository<M3UPlaylist>
    {
        Task <M3UPlaylist> GetByIdAsync(long id);
        Task<List<M3UPlaylist>> GetAllM3uPlaylitsAsync();
    }
}
