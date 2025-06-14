using DocsPortal.BLL.Context;
using DocsPortal.Library;
using Microsoft.AspNetCore.Mvc;

namespace DocsPortal.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly BLContext _context;

        public DocumentsController(BLContext context)
        {
            _context = context;
        }

        [HttpGet("{uid}")]
        public ActionResult<DocumentDTO> GetDocument(Guid uid)
        {
            try 
            {
                var document = _context.DocumentsBL.GetDocument(uid);
                if (document == null)
                {
                    return NotFound();
                }

                return Ok(document);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        public ActionResult<List<DocumentDTO>> GetAllDocuments()
        {
            try 
            {
                var documents = _context.DocumentsBL.GetAllDocuments();
                return Ok(documents);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public ActionResult AddDocument([FromBody] DocumentDTO documentDto)
        {
            if (documentDto == null)
            {
                return BadRequest("Document data is null.");
            }
            try 
            {
                _context.DocumentsBL.AddDocument(documentDto);

                return CreatedAtAction(nameof(GetDocument), new { uid = documentDto.DocumentUid }, documentDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{uid}")]
        public ActionResult UpdateDocument(Guid uid, [FromBody] DocumentDTO documentDto)
        {
            if (documentDto == null || documentDto.DocumentUid != uid)
            {
                return BadRequest("Document data is invalid.");
            }
            try 
            {
                _context.DocumentsBL.UpdateDocument(documentDto);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{uid}")]
        public ActionResult DeleteDocument(Guid uid)
        {
            try 
            {
                _context.DocumentsBL.DeleteDocument(uid);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
