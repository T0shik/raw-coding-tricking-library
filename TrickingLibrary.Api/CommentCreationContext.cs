using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TrickingLibrary.Api.Form;
using TrickingLibrary.Data;
using TrickingLibrary.Models;

namespace TrickingLibrary.Api
{
    public class CommentCreationContext
    {
        private readonly AppDbContext _ctx;
        private static Regex _tagMatch = new Regex(@"\B(?<tag>@[a-zA-Z0-9-_]+)", RegexOptions.Compiled);
        private string _userId;

        public CommentCreationContext(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public class CommentForm
        {
            public int ParentId { get; set; }
            public ParentType ParentType { get; set; }
            public string Content { get; set; }
        }

        public CommentCreationContext Setup(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));

            _userId = userId;

            return this;
        }

        public async Task<Comment> CreateAsync(CommentForm commentForm)
        {
            var comment = new Comment();

            if (commentForm.ParentType == ParentType.ModerationItem)
            {
                if (!_ctx.ModerationItems.Any(x => x.Id == commentForm.ParentId))
                    throw new ParentNotFoundException("Moderation Item not found");
                comment.ModerationItemId = commentForm.ParentId;
            }
            else if (commentForm.ParentType == ParentType.Submission)
            {
                if (!_ctx.Submissions.Any(x => x.Id == commentForm.ParentId))
                    throw new ParentNotFoundException("Submission not found");
                comment.SubmissionId = commentForm.ParentId;
            }
            else if (commentForm.ParentType == ParentType.Comment)
            {
                if (!_ctx.Comments.Any(x => x.Id == commentForm.ParentId))
                    throw new ParentNotFoundException("Comment not found");
                comment.ParentId = commentForm.ParentId;
            }

            comment.Content = commentForm.Content;
            comment.UserId = _userId;
            comment.HtmlContent = _tagMatch.Matches(commentForm.Content)
                .Aggregate(commentForm.Content,
                    (content, match) =>
                    {
                        var tag = match.Groups["tag"].Value;
                        return content
                            .Replace(tag, $"<a href=\"{tag}-user-link\">{tag}</a>");
                    });

            _ctx.Add(comment);
            await _ctx.SaveChangesAsync();

            comment.User = _ctx.Users.AsNoTracking().FirstOrDefault(x => x.Id == _userId);

            return comment;
        }

        public enum ParentType
        {
            ModerationItem = 0,
            Submission = 1,
            Comment = 2,
        }

        public class ParentNotFoundException : Exception
        {
            public ParentNotFoundException(string message) : base(message)
            {
            }
        }
    }
}