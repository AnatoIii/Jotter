using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace BL.Serializer
{
	public class JotterDataContractSerializer<T> : ISerializer<T>
	{
		public void Serialize(T data, string fileName)
		{
			using (var writer = new FileStream(fileName, FileMode.Create)) {
				var serializer = new DataContractSerializer(typeof(T));

				serializer.WriteObject(writer, data);
			}
		}

		public T Deserialize(string fileName)
		{
			if (!File.Exists(fileName)) {
				return default(T);
			}

			using (var fs = new FileStream(fileName, FileMode.Open))
			using (var reader = XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas())) {
				var serializer = new DataContractSerializer(typeof(T));
				return (T)serializer.ReadObject(reader, true);
			}
		}
	}
}
