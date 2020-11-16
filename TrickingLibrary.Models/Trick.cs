using System.Collections.Generic;
using TrickingLibrary.Models.Abstractions;

namespace TrickingLibrary.Models
{
    public class Trick : VersionedModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Difficulty { get; set; }

        public IList<TrickRelationship> Prerequisites { get; set; } = new List<TrickRelationship>();
        public IList<TrickRelationship> Progressions { get; set; } = new List<TrickRelationship>();
        public IList<TrickCategory> TrickCategories { get; set; } = new List<TrickCategory>();
    }
}