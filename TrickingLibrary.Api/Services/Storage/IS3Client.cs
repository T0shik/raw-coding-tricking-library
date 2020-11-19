using System.IO;
using System.Threading.Tasks;

namespace TrickingLibrary.Api.Services.Storage
{
    public interface IS3Client
    {
        Task<string> SaveFile(string fileName, string mime, Stream fileStream);
    }
}