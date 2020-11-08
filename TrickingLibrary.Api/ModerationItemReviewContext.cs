using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TrickingLibrary.Data;
using TrickingLibrary.Models.Moderation;

namespace TrickingLibrary.Api
{
    public class ModerationItemReviewContext
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IOptionsMonitor<ModerationSettings> _moderationOptionsMonitor;
        private readonly SemaphoreSlim _semaphore;

        public ModerationItemReviewContext(
            IServiceProvider serviceProvider,
            IOptionsMonitor<ModerationSettings> moderationOptionsMonitor)
        {
            _serviceProvider = serviceProvider;
            _moderationOptionsMonitor = moderationOptionsMonitor;
            _semaphore = new SemaphoreSlim(1);
        }

        public class ReviewForm
        {
            public string Comment { get; set; }
            public ReviewStatus Status { get; set; }
        }

        public class ModerationSettings
        {
            public int GoalCount { get; set; }
        }

        public class ModerationItemNotFound : Exception
        {
        }

        public async Task Review(int moderationItemId, string userId, ReviewForm form)
        {
            using var scope = _serviceProvider.CreateScope();
            var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var migrationContext = scope.ServiceProvider.GetRequiredService<VersionMigrationContext>();
            var setting = _moderationOptionsMonitor.CurrentValue;
            await _semaphore.WaitAsync();


            var modItem = ctx.ModerationItems
                .Include(x => x.Reviews)
                .FirstOrDefault(x => x.Id == moderationItemId && !x.Deleted);

            if (modItem == null)
            {
                throw new ModerationItemNotFound();
            }

            var review = modItem.Reviews.FirstOrDefault(x => x.UserId == userId);

            if (review == null)
            {
                modItem.Reviews.Add(new Review
                {
                    Comment = form.Comment,
                    Status = form.Status,
                    UserId = userId,
                });
            }
            else
            {
                review.Comment = form.Comment;
                review.Status = form.Status;
            }

            int goal = setting.GoalCount, score = 0, wait = 0;
            foreach (var r in modItem.Reviews)
            {
                if (r.Status == ReviewStatus.Approved)
                    score++;
                else if (r.Status == ReviewStatus.Rejected)
                    score--;
                else if (r.Status == ReviewStatus.Waiting)
                    wait++;
            }

            if (score >= goal + wait)
            {
                migrationContext.Migrate(modItem);
                modItem.Deleted = true;
            }
            else if (score <= -goal - wait)
            {
                // todo cleanup target
                modItem.Deleted = true;
                modItem.Rejected = true;
            }

            modItem.Updated = DateTime.UtcNow;
            await ctx.SaveChangesAsync();

            _semaphore.Release();
        }
    }
}