using TvPlaylistManager.Application.Contracts.Interfaces;
using TvPlaylistManager.Domain.Models.Epg;

namespace TvPlaylistManager.Infrastructure.Data.Repositories
{
    public class EpgRepository : BaseRepository<EpgSource>, IEpgRepository
    {
        public EpgRepository(AppDbContext context) : base(context)
        {
        }
    }
}
