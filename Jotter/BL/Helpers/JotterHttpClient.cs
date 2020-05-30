using BL.Errors;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BL.Helpers
{
	public class JotterHttpClient
	{
		private readonly HttpClient _httpClient;
		private readonly string _serverUrl;

		public JotterHttpClient()
		{
			_httpClient = new HttpClient();
		}

		#region Get

		public async Task<T> Get<T>(string path) where T : class
		{
			var httpResponse = await _httpClient.GetAsync(BuildUrl(path));

			return await GetAndParseMessage<T>(httpResponse, path);
		}

		#endregion

		#region Put

		public async Task<T> Put<T, W>(string path, W data) where T : class
		{
			string json = JsonConvert.SerializeObject(data);

			var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

			var httpResponse = await _httpClient.PutAsync(BuildUrl(path), httpContent);

			return await GetAndParseMessage<T>(httpResponse, path);
		}

		public async Task<T> Put<T>(string path, HttpContent content) where T : class
		{
			var httpResponse = await _httpClient.PutAsync(BuildUrl(path), content);

			return await GetAndParseMessage<T>(httpResponse, path);
		}

		#endregion

		#region Post

		public async Task<T> Post<T, W>(string path, W data) where T : class
		{
			string json = JsonConvert.SerializeObject(data);

			var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

			var httpResponse = await _httpClient.PostAsync(BuildUrl(path), httpContent);

			return await GetAndParseMessage<T>(httpResponse, path);
		}

		public async Task<T> Post<T>(string path, HttpContent content) where T : class
		{
			var httpResponse = await _httpClient.PostAsync(BuildUrl(path), content);

			return await GetAndParseMessage<T>(httpResponse, path);
		}

		#endregion

		#region Delete

		public async Task<T> Delete<T>(string path) where T : class
		{
			var httpResponse = await _httpClient.DeleteAsync(BuildUrl(path));

			return await GetAndParseMessage<T>(httpResponse, path);
		}

		#endregion

		private async Task<T> GetAndParseMessage<T>(HttpResponseMessage httpResponse, string path)
		{
			if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK) {
				throw new HttpException(path);
			}

			var stringResponse = await httpResponse.Content.ReadAsStringAsync();

			return JsonConvert.DeserializeObject<T>(stringResponse);
		}

		private string BuildUrl(string path)
		{
			return $"{_serverUrl}/{path}";
		}
	}
}
