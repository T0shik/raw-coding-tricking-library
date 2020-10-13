namespace TrickingLibrary.Api.Form
{
    public class CommentForm
    {
        public int ParentId { get; set; }
        public CommentCreationContext.ParentType ParentType { get; set; }
        public string Content { get; set; }
    }
}