using System;

namespace TrickingLibrary.Models.Abstractions
{
    public abstract class VersionedModel : BaseModel<int>
    {
        public int Version { get; set; }
        public bool Active { get; set; }
        public DateTime TimeStamp { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}