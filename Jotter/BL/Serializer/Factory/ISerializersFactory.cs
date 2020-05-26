using BL.Serializer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Serializer.Factory
{
	public interface ISerializersFactory<T>
	{
		ISerializer<T> GetSerializer(SerializerType serializerType);
		string GetSerializerData(SerializerType serializerType);
	}
}
