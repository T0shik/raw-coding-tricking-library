using System.IO;
using System.Threading.Tasks;

namespace TrickingLibrary.Api.Services.Storage
{
    public class S3FileProvider : IFileProvider
    {
        private readonly IS3Client _client;

        public S3FileProvider(IS3Client client)
        {
            _client = client;
        }

        public Task<string> SaveProfileImageAsync(Stream fileStream)
        {
            var fileName = TrickingLibraryConstants.Files.GenerateProfileFileName();
            return _client.SaveFile(fileName, "image/jpg", fileStream);
        }

        public Task<string> SaveVideoAsync(Stream fileStream)
        {
            var fileName = TrickingLibraryConstants.Files.GenerateConvertedFileName();
            return _client.SaveFile(fileName, "video/mp4", fileStream);
        }

        public Task<string> SaveThumbnailAsync(Stream fileStream)
        {
            var fileName = TrickingLibraryConstants.Files.GenerateThumbnailFileName();
            return _client.SaveFile(fileName, "image/jpg", fileStream);
        }
    }
}