using System.Collections.Generic;

namespace TrickingLibrary.Models.Moderation
{
    public class ModerationItem : BaseModel<int>
    {
        public string Target { get; set; }
        public string Type { get; set; }
        public IList<Comment> Comments { get; set; } = new List<Comment>();
    }
}