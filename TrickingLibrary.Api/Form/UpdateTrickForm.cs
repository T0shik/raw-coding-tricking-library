namespace TrickingLibrary.Api.Form
{
    public class UpdateTrickForm : CreateTrickForm
    {
        public int Id { get; set; }
        public string Reason { get; set; }
    }
}