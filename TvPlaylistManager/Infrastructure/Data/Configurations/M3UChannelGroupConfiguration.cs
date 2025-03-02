using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TvPlaylistManager.Domain.Models.M3u;

namespace TvPlaylistManager.Infrastructure.Data.Configurations
{
    public class M3UChannelGroupConfiguration : IEntityTypeConfiguration<M3UChannelGroup>
    {
        public void Configure(EntityTypeBuilder<M3UChannelGroup> builder)
        { 
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Name).IsRequired();
            builder.Property(e => e.M3UPlaylistId).IsRequired();

            builder.HasMany(e => e.Channels)
                .WithOne(e => e.ChannelGroup)
                .HasForeignKey(e => e.ChannelGroupId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
