using System.Collections.Generic;
using TrickingLibrary.Models.Abstractions;

namespace TrickingLibrary.Models.Moderation
{
    public class ModerationItem : TemporalModel
    {
        public int Id { get; set; }
        public string Target { get; set; }
        public int TargetVersion { get; set; }
        public string Type { get; set; }
        public IList<Comment> Comments { get; set; } = new List<Comment>();
        public IList<Review> Reviews { get; set; } = new List<Review>();
    }
}