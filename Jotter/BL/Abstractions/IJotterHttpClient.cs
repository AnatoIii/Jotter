using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BL.Abstractions
{
	public interface IJotterHttpClient
	{
		Task<T> Post<T, W>(string path, W data) where T : class;
		Task<T> Post<T>(string path, HttpContent content) where T : class;

		Task<T> Get<T>(string path) where T : class;

		Task<T> Put<T, W>(string path, W data) where T : class;
		Task<T> Put<T>(string path, HttpContent content) where T : class;

		Task<T> Delete<T>(string path) where T : class;
	}
}
