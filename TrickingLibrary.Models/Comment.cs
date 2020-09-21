using System.Collections.Generic;
using TrickingLibrary.Models.Abstractions;
using TrickingLibrary.Models.Moderation;

namespace TrickingLibrary.Models
{
    public class Comment : TemporalModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string HtmlContent { get; set; }

        public int? ModerationItemId { get; set; }
        public ModerationItem ModerationItem { get; set; }

        public int? ParentId { get; set; }
        public Comment Parent { get; set; }

        public IList<Comment> Replies { get; set; } = new List<Comment>();
    }
}