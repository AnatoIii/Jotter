using System.Threading.Tasks;

namespace JotterAPI.Services.Abstractions
{
    public interface IFileServerClient
    {
        Task<string> AddFile(string data, string relativePath);
        Task<string> ReadFile(string relativePath);
        Task DeleteFile(string relativePath);
    }
}