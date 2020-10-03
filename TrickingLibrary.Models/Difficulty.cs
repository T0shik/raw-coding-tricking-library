using System.Collections.Generic;
using TrickingLibrary.Models.Abstractions;

namespace TrickingLibrary.Models
{
    public class Difficulty : BaseModel<string>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<Trick> Tricks { get; set; }
    }
}