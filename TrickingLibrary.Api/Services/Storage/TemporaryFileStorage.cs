using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace TrickingLibrary.Api.Services.Storage
{
    public class TemporaryFileStorage
    {
        private readonly FileSettings _settings;

        public TemporaryFileStorage(IOptionsMonitor<FileSettings> optionsMonitor)
        {
            _settings = optionsMonitor.CurrentValue;
        }
        
        public async Task<string> SaveTemporaryFile(IFormFile video)
        {
            var fileName = string.Concat(
                TrickingLibraryConstants.Files.TempPrefix,
                DateTime.Now.Ticks,
                Path.GetExtension(video.FileName)
            );
            var savePath = GetSavePath(fileName);

            await using (var fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write))
            {
                await video.CopyToAsync(fileStream);
            }

            return fileName;
        }

        public bool TemporaryFileExists(string fileName)
        {
            var path = GetSavePath(fileName);
            return File.Exists(path);
        }

        public void DeleteTemporaryFile(string fileName)
        {
            var path = GetSavePath(fileName);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public string GetSavePath(string fileName)
        {
            return Path.Combine(_settings.WorkingDirectory, fileName);
        }
    }
}