using System;
using System.IO;

namespace JotterAPI.FileServer
{
    public class FileService : IFileService
    {
        private string _baseFileServerPath;

        public FileService(string baseFileServerPath)
        {
            _baseFileServerPath = baseFileServerPath;
        
            if (!Directory.Exists(baseFileServerPath))
            {
                Directory.CreateDirectory(baseFileServerPath);
            }
        }

        public string AddFile(string fileToSave, string relativePath)
        {
            var fullPath = _baseFileServerPath + relativePath;

            using (var streamWriter = new StreamWriter(fullPath)) {
                streamWriter.Write(fileToSave);
            }

            return fullPath;
        }

        public string GetFileById(string path)
        {
            if (!File.Exists(path)) {
                return string.Empty;
            }

            using (var streamReader = new StreamReader(path)) {
                return streamReader.ReadToEnd();
            }
        }

        public void DeleteFile(string path)
        {
            if (File.Exists(path)) {
                File.Delete(path);
            }
        }
    }
}