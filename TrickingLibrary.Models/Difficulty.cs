using System.Collections.Generic;
using TrickingLibrary.Models.Abstractions;

namespace TrickingLibrary.Models
{
    public class Difficulty : VersionedModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<TrickDifficulty> Tricks { get; set; } = new List<TrickDifficulty>();
    }
}