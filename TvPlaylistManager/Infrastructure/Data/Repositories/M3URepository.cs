using Microsoft.EntityFrameworkCore;
using TvPlaylistManager.Domain.Interfaces;
using TvPlaylistManager.Domain.Models.M3u;

namespace TvPlaylistManager.Infrastructure.Data.Repositories
{
    public class M3URepository : BaseRepository<M3UPlaylist>, IM3URepository
    {
        public M3URepository(AppDbContext context) : base(context)
        {            
        }

        public async Task<M3UPlaylist> GetByIdAsync(long id)
        {
            var m3uPlaylist = await _dbSet
                .Include(x => x.EpgSource)
                .Include(x => x.ChannelGroups)
                .ThenInclude(x => x.Channels)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            return m3uPlaylist;
        }

        public async Task<List<M3UPlaylist>> GetAllM3uPlaylitsAsync()
        {
            var m3uPlaylists = await _dbSet
                .Include(x => x.EpgSource)
                .Include(x => x.ChannelGroups)
                .ThenInclude(x => x.Channels)
                .ToListAsync();

            return m3uPlaylists;
        }
    }
}

