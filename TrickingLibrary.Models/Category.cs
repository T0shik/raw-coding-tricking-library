using System.Collections.Generic;
using TrickingLibrary.Models.Abstractions;

namespace TrickingLibrary.Models
{
    public class Category : BaseModel<string>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<TrickCategory> Tricks { get; set; }
    }
}