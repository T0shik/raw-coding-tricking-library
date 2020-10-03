using System.Collections.Generic;
using TrickingLibrary.Models.Abstractions;

namespace TrickingLibrary.Models
{
    public class User : BaseModel<string>
    {
        public string Username { get; set; }

        public string Image { get; set; }
        
        public IList<Submission> Submissions { get; set; } = new List<Submission>();
    }
}