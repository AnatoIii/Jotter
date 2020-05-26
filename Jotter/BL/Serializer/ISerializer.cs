using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Serializer
{
	public interface ISerializer<T>
	{
		void Serialize(T data, string fileName);
		T Deserialize(string fileName);
	}
}
