namespace TrickingLibrary.Api.Services.Storage
{
    public class S3Settings
    {
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string ServiceUrl { get; set; }
        public string Bucket { get; set; }
        public string Root { get; set; }
    }
}