using FileUploader.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FileUploader.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly BlobService _blobService;
        public FilesController(BlobService blobService)
        {
            _blobService = blobService ?? throw new ArgumentNullException(nameof(blobService));
        }

        [HttpGet]
        public async Task<IActionResult> Get() 
        {
            var results = await _blobService.GetBlobsAsync();
            
            return Ok(results);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            
            // TO DO: pass stream to await _blobService.UploadBlobAsync(file.FileName, stream);

            return NoContent();
        }

        [HttpGet("/{name}")]
        public async Task<IActionResult> Download(string name)
        {
            var result = await _blobService.DownloadBlobAsync(name);

            return File(result.Stream, result.ContentType, result.Name);
        }
    }
}
