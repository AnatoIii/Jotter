using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BL.Serializer
{
	public class JotterJsonSerializer<T> : ISerializer<T>
	{
		public void Serialize(T data, string fileName)
		{
			using (var writer = new StreamWriter(fileName)) {
				var serializer = new JsonSerializer();

				serializer.Serialize(writer, data);
			}
		}

		public T Deserialize(string fileName)
		{
			if (!File.Exists(fileName)) {
				return default(T);
			}

			using (StreamReader file = File.OpenText(fileName)) {
				JsonSerializer serializer = new JsonSerializer();

				return (T)serializer.Deserialize(file, typeof(T));
			}
		}
	}
}
