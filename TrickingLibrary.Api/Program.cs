using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TrickingLibrary.Data;
using TrickingLibrary.Models;
using TrickingLibrary.Models.Moderation;

namespace TrickingLibrary.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

                if (env.IsDevelopment())
                {
                    ctx.Add(new Difficulty {Id = "easy", Name = "Easy", Description = "Easy Test"});
                    ctx.Add(new Difficulty {Id = "medium", Name = "Medium", Description = "Medium Test"});
                    ctx.Add(new Difficulty {Id = "hard", Name = "Hard", Description = "Hard Test"});
                    ctx.Add(new Category {Id = "kick", Name = "Kick", Description = "Kick Test"});
                    ctx.Add(new Category {Id = "flip", Name = "Flip", Description = "Flip Test"});
                    ctx.Add(new Category {Id = "transition", Name = "Transition", Description = "Transition Test"});
                    ctx.Add(new Trick
                    {
                        Id = "backwards-roll",
                        Name = "Backwards Roll",
                        Description = "This is a test backwards roll",
                        Difficulty = "easy",
                        TrickCategories = new List<TrickCategory> {new TrickCategory {CategoryId = "flip"}}
                    });
                    ctx.Add(new Trick
                    {
                        Id = "forwards-roll",
                        Name = "Forwards Roll",
                        Description = "This is a test forwards roll",
                        Difficulty = "easy",
                        TrickCategories = new List<TrickCategory> {new TrickCategory {CategoryId = "flip"}}
                    });
                    ctx.Add(new Trick
                    {
                        Id = "back-flip",
                        Name = "Back Flip",
                        Description = "This is a test back flip",
                        Difficulty = "medium",
                        TrickCategories = new List<TrickCategory> {new TrickCategory {CategoryId = "flip"}},
                        Prerequisites = new List<TrickRelationship>
                        {
                            new TrickRelationship {PrerequisiteId = "backwards-roll"}
                        }
                    });
                    ctx.Add(new Submission
                    {
                        TrickId = "back-flip",
                        Description = "Test description, I've tried to go for max height",
                        Video = new Video
                        {
                            VideoLink = "one.mp4",
                            ThumbLink = "one.jpg"
                        },
                        VideoProcessed = true,
                    });
                    ctx.Add(new Submission
                    {
                        TrickId = "back-flip",
                        Description = "Test description, I've tried to go for min height",
                        Video = new Video
                        {
                            VideoLink = "two.mp4",
                            ThumbLink = "two.jpg"
                        },
                        VideoProcessed = true,
                    });
                    ctx.Add(new ModerationItem
                    {
                        Target = "forwards-roll",
                        Type = ModerationTypes.Trick,
                    });
                    ctx.SaveChanges();


                    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                    var user = new IdentityUser("test");
                    userMgr.CreateAsync(user, "password").GetAwaiter().GetResult();
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}