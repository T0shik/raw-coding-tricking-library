using System;

namespace TrickingLibrary.Models.Abstractions
{
    public class Mutable : BaseModel<int>
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public DateTime Updated { get; set; } = DateTime.UtcNow;
    }
}