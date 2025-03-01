using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TvPlaylistManager.Domain.Models.Epg;

namespace TvPlaylistManager.Infrastructure.Data.Configurations
{
    public class EpgSourceConfiguration : IEntityTypeConfiguration<EpgSource>
    {
        public void Configure(EntityTypeBuilder<EpgSource> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Alias).IsRequired();
            builder.Property(e => e.Url).IsRequired();
            builder.Property(e => e.Status).IsRequired();
            builder.Property(e => e.CreatedAt).IsRequired();

            builder.HasMany(e => e.Channels)
                .WithOne(e => e.EpgSource)
                .HasForeignKey(e => e.EpgSourceId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => e.Alias).IsUnique();
            builder.HasIndex(e => e.Url).IsUnique();
        }
    }
}
