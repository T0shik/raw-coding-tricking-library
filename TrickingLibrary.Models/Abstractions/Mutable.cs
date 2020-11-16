using System;

namespace TrickingLibrary.Models.Abstractions
{
    public class Mutable<TKey> : BaseModel<TKey>
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public DateTime Updated { get; set; } = DateTime.UtcNow;
    }
}