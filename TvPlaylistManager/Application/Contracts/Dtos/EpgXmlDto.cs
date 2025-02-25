using System.Xml.Serialization;

namespace TvPlaylistManager.Application.Contracts.Dtos
{
    [XmlRoot("tv")]
    public class EpgXmlDto
    {
        [XmlElement("channel")]
        public List<ChannelDto> Channels { get; set; } = [];
    }

    public class ChannelDto
    {
        [XmlAttribute("id")]
        public string Id { get; set; } = string.Empty;

        [XmlElement("display-name")]
        public string DisplayName { get; set; } = string.Empty;

        [XmlElement("icon")]
        public string Icon { get; set; } = string.Empty;
    }
}
