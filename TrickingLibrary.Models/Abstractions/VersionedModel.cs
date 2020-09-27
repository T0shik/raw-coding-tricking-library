using System;

namespace TrickingLibrary.Models.Abstractions
{
    public abstract class VersionedModel : TemporalModel
    {
        public int Version { get; set; }
        public bool Active { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}