using System;

namespace TrickingLibrary.Models.Abstractions
{
    public abstract class VersionedModel : Mutable<int>
    {
        public string Slug { get; set; }
        public int Version { get; set; }
        public bool Active { get; set; }
    }
}