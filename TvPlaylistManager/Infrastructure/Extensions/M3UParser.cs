using System.Text.RegularExpressions;
using System.Text;
using TvPlaylistManager.Domain.Models.M3u;

namespace TvPlaylistManager.Infrastructure.Extensions
{

    public static partial class M3UParser
    {
        private const string ExtInfPattern = @"#EXTINF:-1(?:\s+tvg-id=""(?<tvgId>[^""]*)"")?(?:\s+tvg-name=""(?<tvgName>[^""]*)"")?(?:\s+tvg-logo=""(?<tvgLogo>[^""]*)"")?(?:\s+group-title=""(?<groupTitle>[^""]*)"")?,(?<name>.+)";

        public static string Serialize(this M3UPlaylist playlist)
        {
            var sb = new StringBuilder();
            sb.AppendLine("#EXTM3U");

            foreach (var group in playlist.ChannelGroups)
            {
                foreach (var channel in group.Channels)
                {
                    sb.Append("#EXTINF:-1");
                    if (!string.IsNullOrEmpty(channel.TvgId)) sb.Append($" tvg-id=\"{channel.TvgId}\"");
                    if (!string.IsNullOrEmpty(channel.TvgName)) sb.Append($" tvg-name=\"{channel.TvgName}\"");
                    if (!string.IsNullOrEmpty(channel.TvgLogo)) sb.Append($" tvg-logo=\"{channel.TvgLogo}\"");
                    if (!string.IsNullOrEmpty(group.Name)) sb.Append($" group-title=\"{group.Name}\"");
                    sb.AppendLine($", {channel.Name}");
                    sb.AppendLine(channel.Url);
                }
            }

            return sb.ToString();
        }

        public static List<M3UChannelGroup> Deserialize(this string m3uContent, bool excludeFilteredChannels)
        {
            var lines = m3uContent.Split('\n')
                .Select(line => line.Trim())
                .Where(line => !string.IsNullOrEmpty(line))
                .ToList();

            if (lines.Count == 0 || !lines[0].StartsWith("#EXTM3U"))
                throw new FormatException("Invalid M3U file.");

            var channelGroups = new List<M3UChannelGroup>();
            var groupsDict = new Dictionary<string, M3UChannelGroup>();

            var filteredTerms = new List<string> { "Adulto", "XXX", "18+", "Hot", "4K", "FHDR", "H265", "Alter"};

            for (int i = 1; i < lines.Count - 1; i++)
            {
                if (lines[i].StartsWith("#EXTINF"))
                {
                    var match = M3uRegex().Match(lines[i]);
                    if (match.Success && i + 1 < lines.Count)
                    {
                        var groupName = match.Groups["groupTitle"].Value;
                        if (string.IsNullOrEmpty(groupName))
                            groupName = "Default";

                        if (!groupsDict.TryGetValue(groupName, out var group))
                        {
                            group = new M3UChannelGroup { Name = groupName, Channels = [] };
                            groupsDict[groupName] = group;
                            channelGroups.Add(group);
                        }

                        var channelName = match.Groups["name"].Value;

                        if (excludeFilteredChannels && filteredTerms.Exists(term => channelName.Contains(term, StringComparison.OrdinalIgnoreCase)))
                        {
                            continue; 
                        }

                        channelName = CleanChannelName(channelName);

                        var channel = new M3UChannel
                        {
                            TvgId = match.Groups["tvgId"].Value,
                            TvgName = match.Groups["tvgName"].Value,
                            TvgLogo = match.Groups["tvgLogo"].Value,
                            Name = channelName,
                            Url = lines[i + 1]
                        };

                        group.Channels.Add(channel);
                        i++;
                    }
                }
            }

            return [.. channelGroups.Where(x => x.Channels.Count > 0)];
        }

        private static string CleanChannelName(string name)
        {
            if (name.EndsWith(" SD", StringComparison.OrdinalIgnoreCase))
                name = name[..^3]; 

            if (name.EndsWith("- SD", StringComparison.OrdinalIgnoreCase))
                name = name[..^4];

            if (name.EndsWith("FHD", StringComparison.OrdinalIgnoreCase))
                name = name[..^3] + "HD";

            return name.Trim();
        }


        [GeneratedRegex(ExtInfPattern)]
        private static partial Regex M3uRegex();
    }
}
