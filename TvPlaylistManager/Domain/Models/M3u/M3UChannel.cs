namespace TvPlaylistManager.Domain.Models.M3u
{
    public class M3UChannel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string TvgId { get; set; }
        public string TvgName { get; set; }
        public string TvgLogo { get; set; }
        public string Url { get; set; }
        public long ChannelGroupId { get; set; }
        public M3UChannelGroup ChannelGroup { get; set; }

    }
}
