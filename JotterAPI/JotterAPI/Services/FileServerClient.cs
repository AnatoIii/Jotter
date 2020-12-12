using System;
using System.Net.Http;
using System.Threading.Tasks;
using JotterAPI.Model;
using JotterAPI.Model.DTOs.Files;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace JotterAPI.Services
{
    public class FileServerClient
    {
        private HttpClient _httpClient;
        private string _fileServerURL;

        public FileServerClient(HttpClient httpClient, IOptions<Hosts> hosts)
        {
            _httpClient = httpClient;
            _fileServerURL = hosts.Value.FileServerURL + "/files";
        }

        public async Task<string> AddFile(string data, string relativePath)
        {
            var requestBody = new SaveFileRequest()
            {
                File = data,
                RelativePath = relativePath
            };
            var request = new HttpRequestMessage(HttpMethod.Post, _fileServerURL);
            request.Content = new StringContent(JsonConvert.SerializeObject(requestBody));
            var response = await _httpClient.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }
        
        public async Task<string> ReadFile(string relativePath)
        {
            var urlBuilder = new UriBuilder(_fileServerURL);
            var queryBuilder = new QueryBuilder {{"path", relativePath}};
            urlBuilder.Query = queryBuilder.ToString();
            var request = new HttpRequestMessage(HttpMethod.Get, urlBuilder.ToString());
            request.Content = new StringContent(relativePath);
            var response = await _httpClient.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }
        
        public async Task DeleteFile(string relativePath)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, _fileServerURL);
            request.Content = new StringContent(relativePath);
            var response = await _httpClient.SendAsync(request);
            await response.Content.ReadAsStringAsync();
        }
    }
}