using AssignmentService.Service;
using AssignmentService.Service.ImpService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Share.Other.SearchModel;

namespace AssignmentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttachmentController : ControllerBase
    {
        readonly IAttachmentService _attachmentService;
        public AttachmentController(IAttachmentService attachmentService)
        {
            _attachmentService = attachmentService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAsync([FromQuery] AttachmentSearch search)
        {
            var result = await _attachmentService.GetAttachmentAsync(search);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("get-file")]
        public async Task<IActionResult> GetFileAsync([FromQuery] AttachmentSearch search)
        {
            var result = await _attachmentService.GetFileAsync(search);
            var fileName = $"attachments-{DateTime.Now:yyyyMMddHHmmss}.zip";

            return File(result, "application/zip", fileName);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var result = await _attachmentService.DeleteAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
