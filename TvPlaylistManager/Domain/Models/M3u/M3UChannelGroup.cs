namespace TvPlaylistManager.Domain.Models.M3u
{
    public class M3UChannelGroup
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<M3UChannel> Channels { get; set; } = [];        
        public long M3UPlaylistId { get; set; }
        public M3UPlaylist M3UPlaylist { get; set; }
    }
}
