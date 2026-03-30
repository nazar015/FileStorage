using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Identity;
using FileUploader.Models;

namespace FileUploader.Services
{
    public class BlobService
    {
        private readonly BlobContainerClient _containerClient;

        public BlobService(BlobContainerClient containerClient)
        {
            _containerClient = containerClient ?? throw new ArgumentNullException(nameof(containerClient));
        }

        public async Task<List<BlobDto>> GetBlobsAsync()
        {
            var blobs = new List<BlobDto>();

            await foreach (var item in _containerClient.GetBlobsAsync())
            {
                blobs.Add(new BlobDto()
                {
                    Name = item.Name,
                    ContentType = item.Properties.ContentType,
                    CreatedOn = item.Properties.CreatedOn
                });
            }

            return blobs;
        }

        public async Task<BlobDownloadDto> DownloadBlobAsync(string blobName)
        {
            var blobClient = _containerClient.GetBlobClient(blobName);

            Console.WriteLine($"Downloading '{blobName}' from container '{_containerClient.Name}'...");

            BlobDownloadResult result = await blobClient.DownloadContentAsync();

            var stream = result.Content.ToStream();

            var properties = await blobClient.GetPropertiesAsync();
            var contentType = properties.Value.ContentType ?? "application/octet-stream";

            return new BlobDownloadDto()
            {
                Stream = stream,
                Name = blobClient.Name,
                ContentType = contentType
            };
        }

        public async Task UploadBlobAsync(string blobName, string contentType, Stream content)
        {
            await _containerClient.CreateIfNotExistsAsync(PublicAccessType.None);

            var blobClient = _containerClient.GetBlobClient(blobName);

            var headers = new BlobHttpHeaders()
            {
                ContentType = contentType
            };
            
            await blobClient.UploadAsync(content, new BlobUploadOptions()
            {
                HttpHeaders = headers
            });

            Console.WriteLine($"Uploaded blob '{blobName}' to container '{_containerClient.Name}'");
        }
    }
}
