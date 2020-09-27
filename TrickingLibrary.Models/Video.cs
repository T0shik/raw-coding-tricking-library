using TrickingLibrary.Models.Abstractions;

namespace TrickingLibrary.Models
{
    public class Video : TemporalModel
    {
        public string VideoLink { get; set; }
        public string ThumbLink { get; set; }
    }
}