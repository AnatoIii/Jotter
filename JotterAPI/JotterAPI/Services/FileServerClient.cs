using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using JotterAPI.Model;
using JotterAPI.Model.DTOs.Files;
using JotterAPI.Services.Abstractions;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace JotterAPI.Services
{
    public class FileServerClient : IFileServerClient
    {
        private HttpClient _httpClient;
        private string _fileServerURL;

        public FileServerClient() { }

        public FileServerClient(IHttpClientFactory clientFactory, IOptions<Hosts> hosts)
        {
            _httpClient = clientFactory.CreateClient();
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
            request.Content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
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