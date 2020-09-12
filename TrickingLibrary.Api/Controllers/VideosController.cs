using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrickingLibrary.Api.BackgroundServices.VideoEditing;

namespace TrickingLibrary.Api.Controllers
{
    [Route("api/videos")]
    public class VideosController : ControllerBase
    {
        private readonly VideoManager _videoManager;

        public VideosController(VideoManager videoManager)
        {
            _videoManager = videoManager;
        }

        [HttpGet("{video}")]
        public IActionResult GetVideo(string video)
        {
            var savePath = _videoManager.GetSavePath(video);
            if (string.IsNullOrEmpty(savePath))
            {
                return BadRequest();
            }

            return new FileStreamResult(new FileStream(savePath, FileMode.Open, FileAccess.Read), "video/*");
        }

        [HttpPost]
        public Task<string> UploadVideo(IFormFile video)
        {
            return _videoManager.SaveTemporaryVideo(video);
        }

        [HttpDelete("{fileName}")]
        public IActionResult DeleteTemporaryVideo(string fileName)
        {
            if (!_videoManager.Temporary(fileName))
            {
                return BadRequest();
            }

            if (!_videoManager.TemporaryFileExists(fileName))
            {
                return NoContent();
            }

            _videoManager.DeleteTemporaryFile(fileName);

            return Ok();
        }
    }
}