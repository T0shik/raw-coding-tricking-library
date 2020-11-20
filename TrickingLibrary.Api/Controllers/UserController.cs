using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using TrickingLibrary.Api.BackgroundServices.VideoEditing;
using TrickingLibrary.Api.Services.Storage;
using TrickingLibrary.Api.Settings;
using TrickingLibrary.Api.ViewModels;
using TrickingLibrary.Data;
using TrickingLibrary.Models;

namespace TrickingLibrary.Api.Controllers
{
    [Route("api/users")]
    [Authorize]
    public class UserController : ApiController
    {
        private readonly AppDbContext _ctx;

        public UserController(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            var userId = UserId;
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

            var user = await _ctx.Users
                .Where(x => x.Id.Equals(userId))
                .Include(x => x.Submissions)
                .ThenInclude(x => x.Votes)
                .Select(UserViewModels.ProfileProjection(Role))
                .FirstOrDefaultAsync();

            if (user != null) return Ok(user);

            var newUser = new User
            {
                Id = userId,
                Username = Username,
            };

            _ctx.Add(newUser);
            await _ctx.SaveChangesAsync();

            return Ok(UserViewModels.ProfileProjection(Role).Compile().Invoke(newUser));
        }

        [AllowAnonymous]
        [HttpGet("{username}")]
        public object GetUser(string username) =>
            _ctx.Users
                .Where(x => x.Username.ToLower() == username.ToLower())
                .Include(x => x.Submissions)
                .ThenInclude(x => x.Votes)
                .Select(UserViewModels.Projection)
                .FirstOrDefault();

        [AllowAnonymous]
        [HttpGet("{id}/submissions")]
        public Task<List<object>> GetUserSubmissions(string id, [FromQuery] FeedQuery feedQuery) =>
            _ctx.Submissions
                .Include(x => x.Video)
                .Include(x => x.User)
                .Where(x => x.UserId.Equals(id))
                .OrderFeed(feedQuery)
                .Select(SubmissionViewModels.Projection)
                .ToListAsync();

        [HttpPut("me/image")]
        public async Task<IActionResult> UpdateProfileImage(
            IFormFile image,
            [FromServices] IFileProvider fileManager)
        {
            if (image == null) return BadRequest();

            var userId = UserId;
            var user = await _ctx.Users.FirstOrDefaultAsync(x => x.Id.Equals(userId));

            if (user == null) return NoContent();

            await using (var stream = new MemoryStream())
            using (var imageProcessor = await Image.LoadAsync(image.OpenReadStream()))
            {
                imageProcessor.Mutate(x => x.Resize(48, 48));
                var processImage = imageProcessor.SaveAsync(stream, new JpegEncoder());
                var saveImage = fileManager.SaveProfileImageAsync(stream);
                await Task.WhenAll(processImage, saveImage);
                user.Image = await saveImage;
            }

            await _ctx.SaveChangesAsync();
            return Ok();
        }
    }
}