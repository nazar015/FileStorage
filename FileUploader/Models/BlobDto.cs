namespace FileUploader.Models
{
    public class BlobDto
    {
        public string Name { get; set; }
        public string ContentType { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
    }
}
