using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TrickingLibrary.Api.Settings;

namespace TrickingLibrary.Api.BackgroundServices.VideoEditing
{
    public interface IFileManager
    {
        string TemporarySavePath(string messageInput);
        string GetFFMPEGPath();
        string GetFileUrl(string fileName, FileType fileType);
        bool TemporaryFileExists(string outputConvertedName);
        void DeleteTemporaryFile(string outputConvertedName);
        string GetSavePath(string fileName);
        Task<string> SaveTemporaryFile(IFormFile video);
        bool Temporary(string fileName);
    }
}