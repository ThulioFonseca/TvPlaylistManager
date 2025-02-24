namespace TvPlaylistManager.Domain.Models.Epg
{
    public class EpgSource
    {
        public long Id { get; set; }
        public required string Alias { get; set; }
        public required string Url { get; set; }
        public required bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<EpgChannel> Channels { get; set; } = [];
    }
}
