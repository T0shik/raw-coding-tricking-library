using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace TrickingLibrary.Api.BackgroundServices.VideoEditing
{
    public class VideoManager
    {
        private readonly IWebHostEnvironment _env;
        public const string TempPrefix = "temp_";
        public const string ConvertedPrefix = "c";
        public const string ThumbnailPrefix = "t";

        public VideoManager(IWebHostEnvironment env)
        {
            _env = env;
        }

        public string WorkingDirectory => _env.WebRootPath;

        public bool Temporary(string fileName)
        {
            return fileName.StartsWith(TempPrefix);
        }

        public bool TemporaryVideoExists(string fileName)
        {
            var path = TemporarySavePath(fileName);
            return File.Exists(path);
        }

        public void DeleteTemporaryVideo(string fileName)
        {
            var path = TemporarySavePath(fileName);
            File.Delete(path);
        }

        public string DevVideoPath(string fileName)
        {
            return !_env.IsDevelopment() ? null : Path.Combine(WorkingDirectory, fileName);
        }

        public string GenerateConvertedFileName() => $"{ConvertedPrefix}{DateTime.Now.Ticks}.mp4";
        public string GenerateThumbnailFileName() => $"{ThumbnailPrefix}{DateTime.Now.Ticks}.png";

        public async Task<string> SaveTemporaryVideo(IFormFile video)
        {
            var fileName = string.Concat(TempPrefix, DateTime.Now.Ticks, Path.GetExtension(video.FileName));
            var savePath = TemporarySavePath(fileName);

            await using (var fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write))
            {
                await video.CopyToAsync(fileStream);
            }

            return fileName;
        }

        public string TemporarySavePath(string fileName)
        {
            return Path.Combine(WorkingDirectory, fileName);
        }
    }
}