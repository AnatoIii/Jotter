using BL.Serializer.Model;
using Model;
using Model.ModelData;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Serializer.Factory
{
	public class SerializerFactory<T> : ISerializersFactory<T>
	{
		private static Dictionary<SerializerType, Func<ISerializer<T>>> _serializers = new Dictionary<SerializerType, Func<ISerializer<T>>> {
			{ SerializerType.DataContract, () => new JotterDataContractSerializer<T>() },
			{ SerializerType.Json, () => new JotterJsonSerializer<T>() },
			{ SerializerType.Xml, () => new JotterXmlSerializer<T>() }
		};
		
		private static Dictionary<SerializerType, string> _extensions = new Dictionary<SerializerType, string> {
			{ SerializerType.DataContract, ".dt.xml" },
			{ SerializerType.Json, "json" },
			{ SerializerType.Xml, ".xml" }
		};

		private static Dictionary<Type, string> _fileNames = new Dictionary<Type, string> {
			{ typeof(UserCredentials), "data" },
			{ typeof(IEnumerable<Note>), "notes" },
			{ typeof(IEnumerable<Category>), "categories" }
		};

		public ISerializer<T> GetSerializer(SerializerType serializerType)
		{
			return _serializers[serializerType]();
		}

		public string GetSerializerData(SerializerType serializerType)
		{
			return $"{_fileNames[typeof(T)]}{_extensions[serializerType]}";
		}
	}
}
