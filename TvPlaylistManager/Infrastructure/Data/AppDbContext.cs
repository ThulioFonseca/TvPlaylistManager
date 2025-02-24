using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TvPlaylistManager.Domain.Models.Epg;

namespace TvPlaylistManager.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<EpgSource> EpgSources { get; set; }

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
