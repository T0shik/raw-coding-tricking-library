using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
                    var fakeCounter = 20;
                    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                    var testUser = new IdentityUser("test") {Id = "test_user_id", Email = "test@test.com"};
                    userMgr.CreateAsync(testUser, "password").GetAwaiter().GetResult();
                    ctx.Add(new User
                    {
                        Id = testUser.Id,
                        Username = testUser.UserName,
                        Image = "https://localhost:5001/api/files/image/user.jpg"
                    });

                    var fakeUsers = Enumerable.Range(0, fakeCounter)
                        .Select(i => new IdentityUser($"fake{i}") {Id = $"fake_{i}_id", Email = $"fake{i}@test.com"})
                        .ToList();

                    foreach (var fakeUser in fakeUsers)
                    {
                        userMgr.CreateAsync(fakeUser, "password").GetAwaiter().GetResult();
                        ctx.Add(new User
                        {
                            Id = fakeUser.Id,
                            Username = fakeUser.UserName,
                        });
                    }

                    var mod = new IdentityUser("mod") {Id = "mod_user_id", Email = "mod@test.com"};
                    userMgr.CreateAsync(mod, "password").GetAwaiter().GetResult();
                    userMgr.AddClaimAsync(mod,
                            new Claim(TrickingLibraryConstants.Claims.Role,
                                TrickingLibraryConstants.Roles.Mod))
                        .GetAwaiter()
                        .GetResult();

                    ctx.Add(new User
                    {
                        Id = mod.Id,
                        Username = mod.UserName,
                        Image = "https://localhost:5001/api/files/image/judge.jpg",
                    });
                    var difficulties = new List<Difficulty>
                    {
                        new Difficulty {Id = 1, Slug = "easy", Name = "Easy", Description = "Easy Test", State = VersionState.Live},
                        new Difficulty {Id = 2, Slug = "medium", Name = "Medium", Description = "Medium Test", State = VersionState.Live},
                        new Difficulty {Id = 3, Slug = "hard", Name = "Hard", Description = "Hard Test", State = VersionState.Live},
                    };
                    ctx.AddRange(difficulties);
                    ctx.Add(new Difficulty {Id = 4, Slug = "wtf", Name = "WTF", Description = "Difficulty under moderation", UserId = testUser.Id});
                    var categories = new List<Category>
                    {
                        new Category {Id = 1, Slug = "kick", Name = "Kick", Description = "Kick Test", State = VersionState.Live},
                        new Category {Id = 2, Slug = "flip", Name = "Flip", Description = "Flip Test", State = VersionState.Live},
                        new Category {Id = 3, Slug = "transition", Name = "Transition", Description = "Transition Test", State = VersionState.Live},
                    };
                    ctx.AddRange(categories);
                    ctx.Add(new Category {Id = 4, Slug = "ground-work", Name = "Ground Work", Description = "Category under moderation", UserId = testUser.Id});
                    ctx.Add(new Trick
                    {
                        Id = 1,
                        UserId = testUser.Id,
                        Slug = "backwards-roll",
                        Name = "Backwards Roll",
                        State = VersionState.Live,
                        Version = 1,
                        Description = "This is a test backwards roll",
                        TrickDifficulties = new List<TrickDifficulty> {new TrickDifficulty {DifficultyId = 1, Active = true}},
                        TrickCategories = new List<TrickCategory> {new TrickCategory {CategoryId = 2, Active = true}}
                    });
                    ctx.Add(new Trick
                    {
                        Id = 2,
                        UserId = testUser.Id,
                        Slug = "forwards-roll",
                        Name = "Forwards Roll",
                        State = VersionState.Live,
                        Version = 1,
                        Description = "This is a test forwards roll",
                        TrickDifficulties = new List<TrickDifficulty> {new TrickDifficulty {DifficultyId = 1, Active = true}},
                        TrickCategories = new List<TrickCategory> {new TrickCategory {CategoryId = 2, Active = true}}
                    });
                    ctx.Add(new Trick
                    {
                        Id = 3,
                        UserId = testUser.Id,
                        Slug = "back-flip",
                        Name = "Back Flip",
                        State = VersionState.Live,
                        Version = 1,
                        Description = "This is a test back flip",
                        TrickDifficulties = new List<TrickDifficulty> {new TrickDifficulty {DifficultyId = 2, Active = true}},
                        TrickCategories = new List<TrickCategory> {new TrickCategory {CategoryId = 2, Active = true}},
                        Prerequisites = new List<TrickRelationship>
                        {
                            new TrickRelationship {PrerequisiteId = 1, Active = true},
                        }
                    });
                    ctx.Add(new Trick
                    {
                        Id = 4,
                        UserId = testUser.Id,
                        Slug = "back-flip-360",
                        Name = "Back Flip 360",
                        State = VersionState.Staged,
                        Version = 1,
                        Description = "This is a test back flip 360",
                        TrickDifficulties = new List<TrickDifficulty> {new TrickDifficulty {DifficultyId = 2}},
                        TrickCategories = new List<TrickCategory> {new TrickCategory {CategoryId = 2}},
                        Prerequisites = new List<TrickRelationship>
                        {
                            new TrickRelationship {PrerequisiteId = 3},
                        }
                    });
                    ctx.Add(new Submission
                    {
                        TrickId = "back-flip",
                        Description = "Test description, I've tried to go for max height",
                        Video = new Video
                        {
                            VideoLink = "https://localhost:5001/api/files/video/one.mp4",
                            ThumbLink = "https://localhost:5001/api/files/image/one.jpg"
                        },
                        VideoProcessed = true,
                        UserId = testUser.Id,
                        Votes = new List<SubmissionVote>
                        {
                            new SubmissionVote
                            {
                                UserId = testUser.Id,
                                Value = 1,
                            },
                        },
                    });
                    ctx.Add(new Submission
                    {
                        TrickId = "back-flip",
                        Description = "Test description, I've tried to go for min height",
                        Video = new Video
                        {
                            VideoLink = "https://localhost:5001/api/files/video/two.mp4",
                            ThumbLink = "https://localhost:5001/api/files/image/two.jpg"
                        },
                        VideoProcessed = true,
                        UserId = testUser.Id,
                    });
                    ctx.Add(new ModerationItem
                    {
                        Target = 4,
                        Type = ModerationTypes.Trick,
                        UserId = testUser.Id,
                    });
                    ctx.Add(new ModerationItem
                    {
                        Target = 4,
                        Type = ModerationTypes.Category,
                        UserId = testUser.Id,
                    });
                    ctx.Add(new ModerationItem
                    {
                        Target = 4,
                        Type = ModerationTypes.Difficulty,
                        UserId = testUser.Id,
                    });
                    ctx.SaveChanges();

                    for (var i = 1; i <= fakeCounter; i++)
                    {
                        ctx.Add(new Submission
                        {
                            TrickId = "back-flip",
                            Description = $"Fake submission {i}",
                            Video = new Video
                            {
                                VideoLink = "https://localhost:5001/api/files/video/two.mp4",
                                ThumbLink = "https://localhost:5001/api/files/image/two.jpg"
                            },
                            VideoProcessed = true,
                            UserId = testUser.Id,
                            Created = DateTime.UtcNow.AddDays(-i),
                            Votes = Enumerable
                                .Range(0, i)
                                .Select(ii => new SubmissionVote
                                {
                                    UserId = fakeUsers[ii].Id,
                                    Value = 1,
                                })
                                .ToList(),
                            Comments = Enumerable
                                .Range(0, fakeCounter)
                                .Select(ii => new Comment
                                {
                                    Content = $"Main Comment {ii}",
                                    HtmlContent = $"Main Comment {ii}",
                                    UserId = fakeUsers[ii].Id,
                                    Replies = Enumerable
                                        .Range(0, fakeCounter)
                                        .Select(iii => new Comment
                                        {
                                            Content = $"Reply {iii}",
                                            HtmlContent = $"Reply {iii}",
                                            UserId = fakeUsers[iii].Id,
                                        })
                                        .ToList()
                                })
                                .ToList()
                        });
                    }

                    ctx.SaveChanges();

                    for (var i = 0; i < fakeCounter; i++)
                    {
                        var trick = new Trick
                        {
                            UserId = testUser.Id,
                            Slug = $"fake-trick-{i}",
                            Name = $"Fake Trickery Trick {i}",
                            State = VersionState.Live,
                            Version = 1,
                            Description = $"This is a really fake trick # {i}",
                            TrickDifficulties = new List<TrickDifficulty>
                            {
                                new TrickDifficulty {DifficultyId = difficulties[i % difficulties.Count].Id, Active = true},
                            },
                            TrickCategories = new List<TrickCategory>
                            {
                                new TrickCategory {CategoryId = categories[i % categories.Count].Id, Active = true},
                            },
                        };
                        ctx.Add(trick);
                        ctx.Add(new Submission
                        {
                            TrickId = trick.Slug,
                            Description = $"Fake submission IIII {i}",
                            Video = new Video
                            {
                                VideoLink = "https://localhost:5001/api/files/video/three.mp4",
                                ThumbLink = "https://localhost:5001/api/files/image/three.jpg",
                            },
                            VideoProcessed = true,
                            UserId = testUser.Id,
                            Created = DateTime.UtcNow.AddDays(-i),
                            Votes = Enumerable
                                .Range(0, i)
                                .Select(ii => new SubmissionVote
                                {
                                    UserId = fakeUsers[ii].Id,
                                    Value = 1,
                                })
                                .ToList(),
                        });
                        ctx.SaveChanges();
                    }
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}