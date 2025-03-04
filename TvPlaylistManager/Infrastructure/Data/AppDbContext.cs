using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TvPlaylistManager.Domain.Models.Epg;
using TvPlaylistManager.Domain.Models.M3u;

namespace TvPlaylistManager.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<EpgSource> EpgSources { get; set; }
        public DbSet<M3UPlaylist> M3UPlaylist { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source= Infrastructure/Data/SQLite/TvPlaylistManager.db");
            }
        }
    }
}
