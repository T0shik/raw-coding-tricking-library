using System.IO;
using System.Threading.Tasks;
using TrickingLibrary.Api.Settings;

namespace TrickingLibrary.Api.Services.Storage
{
    public interface IFileProvider
    {
        public Task<string> SaveProfileImageAsync(Stream fileStream);
        public Task<string> SaveVideoAsync(Stream fileStream);
        public Task<string> SaveThumbnailAsync(Stream fileStream);
    }
}