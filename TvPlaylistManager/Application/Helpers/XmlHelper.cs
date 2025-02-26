using System.Xml.Serialization;

namespace TvPlaylistManager.Application.Helpers
{
    public static class XmlHelper
    {
        /// <summary>
        /// Serialize an object to XML.
        /// </summary>
        public static string Serialize<T>(T obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            var serializer = new XmlSerializer(typeof(T));
            using var stringWriter = new StringWriter();
            serializer.Serialize(stringWriter, obj);
            return stringWriter.ToString();
        }

        /// <summary>
        /// Desserialize an XML to an Object.
        /// </summary>
        public static T Deserialize<T>(string xml)
        {
            if (string.IsNullOrWhiteSpace(xml)) throw new ArgumentException("Invalid XML!", nameof(xml));

            var serializer = new XmlSerializer(typeof(T));
            using var stringReader = new StringReader(xml);
            return (T)serializer.Deserialize(stringReader)!;
        }

        /// <summary>
        /// Desserialize a XML from an Stream.
        /// </summary>
        public static T DeserializeFromStream<T>(Stream stream)
        {
            if (stream == null) throw new ArgumentException("Invalid Stream!", nameof(stream));

            var serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(stream)!;
        }
    }
}
