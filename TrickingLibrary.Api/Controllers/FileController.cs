using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrickingLibrary.Api.BackgroundServices.VideoEditing;
using TrickingLibrary.Api.Services.Storage;
using TrickingLibrary.Api.Settings;

namespace TrickingLibrary.Api.Controllers
{
    [Route("api/files")]
    public class FileController : ControllerBase
    {
        private readonly TemporaryFileStorage _temporaryFileStorage;

        public FileController(TemporaryFileStorage temporaryFileStorage)
        {
            _temporaryFileStorage = temporaryFileStorage;
        }

        [HttpGet("{type}/{file}")]
        public IActionResult GetVideo(string type, string file)
        {
            var mime = type.Equals(nameof(FileType.Image), StringComparison.InvariantCultureIgnoreCase)
                ? "image/jpg"
                : type.Equals(nameof(FileType.Video), StringComparison.InvariantCultureIgnoreCase)
                    ? "video/mp4"
                    : null;

            if (mime == null)
            {
                return BadRequest();
            }

            var savePath = _temporaryFileStorage.GetSavePath(file);
            if (string.IsNullOrEmpty(savePath))
            {
                return BadRequest();
            }

            return new FileStreamResult(new FileStream(savePath, FileMode.Open, FileAccess.Read), mime);
        }

        [HttpPost]
        public Task<string> UploadVideo(IFormFile video)
        {
            return _temporaryFileStorage.SaveTemporaryFile(video);
        }

        [HttpDelete("{fileName}")]
        public IActionResult DeleteTemporaryVideo(string fileName)
        {
            if (!_temporaryFileStorage.TemporaryFileExists(fileName))
            {
                return NoContent();
            }

            _temporaryFileStorage.DeleteTemporaryFile(fileName);

            return Ok();
        }
    }
}