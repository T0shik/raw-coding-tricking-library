using System.Collections.Generic;

namespace TrickingLibrary.Api.Form
{
    public class CreateTrickForm
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Difficulty { get; set; }
        public IEnumerable<int> Prerequisites { get; set; }
        public IEnumerable<int> Progressions { get; set; }
        public IEnumerable<int> Categories { get; set; }
    }
}