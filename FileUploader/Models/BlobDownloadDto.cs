namespace FileUploader.Models
{
    public class BlobDownloadDto
    {
        public string Name { get; set; }
        public string ContentType { get; set; }
        public Stream Stream { get; set; }
    }
}
