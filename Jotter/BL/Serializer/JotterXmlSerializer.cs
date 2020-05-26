using System;
using System.IO;
using System.Xml.Serialization;

namespace BL.Serializer
{
	public class JotterXmlSerializer<T> : ISerializer<T>
	{
		public void Serialize(T data, string fileName)
		{
            using (var writer = new StreamWriter(fileName)) {
                var serializer = new XmlSerializer(typeof(T));

				serializer.Serialize(writer, data);
			}
        }

		public T Deserialize(string fileName)
		{
			if (!File.Exists(fileName)) {
				return default(T);
			}

			using (var fileStream = new FileStream(fileName, FileMode.Open)) {
                XmlSerializer serializer = new XmlSerializer(typeof(T));

                return (T)serializer.Deserialize(fileStream);
            }
        }
	}
}
