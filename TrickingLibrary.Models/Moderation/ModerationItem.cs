using System.Collections.Generic;
using TrickingLibrary.Models.Abstractions;

namespace TrickingLibrary.Models.Moderation
{
    public class ModerationItem : BaseModel<int>
    {
        public int Current { get; set; }
        public int Target { get; set; }
        public string Type { get; set; }
        public IList<Comment> Comments { get; set; } = new List<Comment>();
        public IList<Review> Reviews { get; set; } = new List<Review>();
    }
}