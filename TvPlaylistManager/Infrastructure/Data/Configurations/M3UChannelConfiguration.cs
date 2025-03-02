using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TvPlaylistManager.Domain.Models.M3u;

namespace TvPlaylistManager.Infrastructure.Data.Configurations
{
    public class M3UChannelConfiguration : IEntityTypeConfiguration<M3UChannel>
    {
        public void Configure(EntityTypeBuilder<M3UChannel> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Name).IsRequired();
            builder.Property(e => e.TvgId).IsRequired(false);
            builder.Property(e => e.TvgName).IsRequired(false);
            builder.Property(e => e.TvgLogo).IsRequired(false);
            builder.Property(e => e.Url).IsRequired();
            builder.Property(e => e.ChannelGroupId).IsRequired();

            builder.HasOne(e => e.ChannelGroup)
                .WithMany(e => e.Channels)
                .HasForeignKey(e => e.ChannelGroupId);

        }
    }
}
