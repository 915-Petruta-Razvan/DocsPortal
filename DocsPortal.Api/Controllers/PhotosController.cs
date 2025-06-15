using DocsPortal.BLL.Context;
using Microsoft.AspNetCore.Mvc;

namespace DocsPortal.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly BLContext _context;

        public PhotosController(BLContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> ListPhotos()
        {
            var photos = await _context.PhotosBL.ListPhotosAsync();
            return Ok(photos);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadPhoto(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            if (!IsImageFile(file.FileName))
                return BadRequest("Only image files are allowed");

            using var stream = file.OpenReadStream();
            var result = await _context.PhotosBL.UploadPhotoAsync(stream, file.FileName);

            return Ok(new { fileName = file.FileName, result });
        }

        [HttpGet("{blobName}")]
        public async Task<IActionResult> GetPhoto(string blobName)
        {
            try
            {
                var (content, contentType) = await _context.PhotosBL.DownloadPhotoAsync(blobName);
                return File(content, contentType ?? "application/octet-stream");
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{blobName}")]
        public async Task<IActionResult> DeletePhoto(string blobName)
        {
            await _context.PhotosBL.DeletePhotoAsync(blobName);
            return NoContent();
        }

        private bool IsImageFile(string fileName)
        {
            string ext = Path.GetExtension(fileName).ToLowerInvariant();
            return ext switch
            {
                ".jpg" or ".jpeg" or ".png" or ".gif" or ".bmp" => true,
                _ => false
            };
        }
    }
}
