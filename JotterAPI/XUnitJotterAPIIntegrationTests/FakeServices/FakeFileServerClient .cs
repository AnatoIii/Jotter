using JotterAPI.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XUnitJotterAPIIntegrationTests.FakeServices
{
    public class FakeFileServerClient : IFileServerClient
    {
        static string LastSavedFile { get; set; }


        public async Task<string> AddFile(string data, string relativePath)
        {
            await Task.Delay(1);
            LastSavedFile = data;
            return $"dasfasfas{relativePath}";
        }

        public async Task DeleteFile(string relativePath)
        {
            await Task.Delay(1);
        }

        public async Task<string> ReadFile(string relativePath)
        {
            await Task.Delay(1);
            return LastSavedFile;
        }
    }
}
