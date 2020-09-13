using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TrickingLibrary.Api.Settings;
using TrickingLibrary.Data;
using TrickingLibrary.Models;

namespace TrickingLibrary.Api.BackgroundServices.VideoEditing
{
    public class VideoEditingBackgroundService : BackgroundService
    {
        private readonly ILogger<VideoEditingBackgroundService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IFileManager _fileManagerLocal;
        private readonly ChannelReader<EditVideoMessage> _channelReader;

        public VideoEditingBackgroundService(
            Channel<EditVideoMessage> channel,
            ILogger<VideoEditingBackgroundService> logger,
            IServiceProvider serviceProvider,
            IFileManager fileManagerLocal)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _fileManagerLocal = fileManagerLocal;
            _channelReader = channel.Reader;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (await _channelReader.WaitToReadAsync(stoppingToken))
            {
                var message = await _channelReader.ReadAsync(stoppingToken);
                var inputPath = _fileManagerLocal.TemporarySavePath(message.Input);
                var outputConvertedName = TrickingLibraryConstants.Files.GenerateConvertedFileName();
                var outputThumbnailName = TrickingLibraryConstants.Files.GenerateThumbnailFileName();
                var outputConvertedPath = _fileManagerLocal.TemporarySavePath(outputConvertedName);
                var outputThumbnailPath = _fileManagerLocal.TemporarySavePath(outputThumbnailName);
                try
                {
                    var startInfo = new ProcessStartInfo
                    {
                        FileName = _fileManagerLocal.GetFFMPEGPath(),
                        Arguments = $"-y -i {inputPath} -an -vf scale=540x380 {outputConvertedPath} -ss 00:00:00 -vframes 1 -vf scale=540x380 {outputThumbnailPath}",
                        CreateNoWindow = true,
                        UseShellExecute = false,
                    };

                    using (var process = new Process {StartInfo = startInfo})
                    {
                        process.Start();
                        process.WaitForExit();
                    }

                    if (!_fileManagerLocal.TemporaryFileExists(outputConvertedName))
                    {
                        throw new Exception("FFMPEG failed to generate converted video");
                    }

                    if (!_fileManagerLocal.TemporaryFileExists(outputThumbnailName))
                    {
                        throw new Exception("FFMPEG failed to generate thumbnail");
                    }

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                        var submission = ctx.Submissions.FirstOrDefault(x => x.Id.Equals(message.SubmissionId));

                        submission.Video = new Video
                        {
                            VideoLink = _fileManagerLocal.GetFileUrl(outputConvertedName, FileType.Video),
                            ThumbLink = _fileManagerLocal.GetFileUrl(outputThumbnailName, FileType.Image),
                        };
                        submission.VideoProcessed = true;

                        await ctx.SaveChangesAsync(stoppingToken);
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Video Processing Failed for {0}", message.Input);
                    _fileManagerLocal.DeleteTemporaryFile(outputConvertedName);
                    _fileManagerLocal.DeleteTemporaryFile(outputThumbnailName);
                }
                finally
                {
                    _fileManagerLocal.DeleteTemporaryFile(message.Input);
                }
            }
        }
    }
}