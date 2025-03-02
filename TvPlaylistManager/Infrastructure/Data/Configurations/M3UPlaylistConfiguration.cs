using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TvPlaylistManager.Domain.Models.M3u;

namespace TvPlaylistManager.Infrastructure.Data.Configurations
{
    public class M3UPlaylistConfiguration : IEntityTypeConfiguration<M3UPlaylist>
    {
        public void Configure(EntityTypeBuilder<M3UPlaylist> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Name).IsRequired();
            builder.Property(e => e.Url).IsRequired();
            builder.Property(e => e.EpgSourceId).IsRequired(false);
            builder.Property(e => e.CreatedAt).IsRequired();
            builder.Property(e => e.UpdatedAt).IsRequired();

            builder.HasMany(e => e.ChannelGroups)
                .WithOne(e => e.M3UPlaylist)
                .HasForeignKey(e => e.M3UPlaylistId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.EpgSource)
                .WithMany()
                .HasForeignKey(e => e.EpgSourceId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(e => e.Name).IsUnique();
            builder.HasIndex(e => e.Url).IsUnique();
        }
    }
}
