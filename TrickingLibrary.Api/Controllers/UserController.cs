using System;
using System.Collections.Generic;
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
                .Select(UserViewModels.ProfileProjection(IsMod))
                .FirstOrDefaultAsync();

            if (user != null) return Ok(user);

            var newUser = new User
            {
                Id = userId,
                Username = Username,
            };

            _ctx.Add(newUser);
            await _ctx.SaveChangesAsync();

            return Ok(UserViewModels.CreateProfile(newUser, IsMod));
        }

        [AllowAnonymous]
        [HttpGet("{username}")]
        public object GetUser(string username) =>
            _ctx.Users
                .Where(x => x.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase))
                .Select(UserViewModels.FlatProjection)
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
            [FromServices] IFileManager fileManager)
        {
            if (image == null) return BadRequest();

            var userId = UserId;
            var user = await _ctx.Users.FirstOrDefaultAsync(x => x.Id.Equals(userId));

            if (user == null) return NoContent();

            var fileName = TrickingLibraryConstants.Files.GenerateProfileFileName();
            await using (var stream = System.IO.File.Create(fileManager.GetSavePath(fileName)))
            using (var imageProcessor = await Image.LoadAsync(image.OpenReadStream()))
            {
                imageProcessor.Mutate(x => x.Resize(48, 48));

                await imageProcessor.SaveAsync(stream, new JpegEncoder());
            }

            user.Image = fileManager.GetFileUrl(fileName, FileType.Image);
            await _ctx.SaveChangesAsync();
            return Ok(user);
        }
    }
}