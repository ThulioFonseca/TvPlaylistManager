using System.Text.Json;

namespace TvPlaylistManager.Application.Helpers
{
    public static class JsonHelper
    {
        private static readonly JsonSerializerOptions DefaultOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        private static readonly JsonSerializerOptions IndentedOptions = new()
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        private static readonly JsonSerializerOptions DeserializeOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        /// <summary>
        /// Serialize an object to a JSON string.
        /// </summary>
        public static string Serialize<T>(T obj, bool indented = false)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            var options = indented ? IndentedOptions : DefaultOptions;
            return JsonSerializer.Serialize(obj, options);
        }

        /// <summary>
        /// Desserialize a JSON string to an object.
        /// </summary>
        public static T Deserialize<T>(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) throw new ArgumentException("Invalid JSON!", nameof(json));

            return JsonSerializer.Deserialize<T>(json, DeserializeOptions)!;
        }
    }
}
