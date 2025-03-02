namespace TvPlaylistManager.Domain.Models.Epg
{
    public class EpgChannel
    {
        public long Id { get; set; }
        public required string Name { get; set; }
        public required string ChannelEpgId { get; set; }
        public string IconUrl { get; set; }
        public string Keywords{ get; set; }
        public long EpgSourceId { get; set; }
        public required EpgSource EpgSource { get; set; }
    }
}
