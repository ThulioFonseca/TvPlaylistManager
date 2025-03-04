using TvPlaylistManager.Domain.Models.Epg;

namespace TvPlaylistManager.Domain.Models.M3u
{
    public class M3UPlaylist
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public List<M3UChannelGroup> ChannelGroups { get; set; } = [];
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long? EpgSourceId { get; set; }
        public EpgSource EpgSource { get; set; }
    }
}
