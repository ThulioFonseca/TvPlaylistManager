using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TvPlaylistManager.Domain.Models.Epg;

namespace TvPlaylistManager.Infrastructure.Data.Configurations
{
    public class EpgChannelConfiguration : IEntityTypeConfiguration<EpgChannel>
    {
        public void Configure(EntityTypeBuilder<EpgChannel> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Name).IsRequired();
            builder.Property(e => e.ChannelEpgId).IsRequired();
            builder.Property(e => e.IconUrl).IsRequired(false);
            builder.Property(e => e.EpgSourceId).IsRequired();

            builder.HasOne(e => e.EpgSource)
                .WithMany(e => e.Channels)
                .HasForeignKey(e => e.EpgSourceId);

            builder.HasIndex(e => e.ChannelEpgId);
        }
    }
}
