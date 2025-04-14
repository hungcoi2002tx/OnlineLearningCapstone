using AssignmentService.Service;
using AssignmentService.Service.ImpService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Share.Other.SearchModel;
using Share.RequestModel;

namespace AssignmentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubmissionController : ControllerBase
    {
        private readonly ISubmissionService _submissionService;

        public SubmissionController(ISubmissionService submissionService)
        {
            _submissionService = submissionService;
        }

        [HttpPost("quiz")]
        public async Task<IActionResult> CreateQuizSubmissionAsync([FromBody] CreateQuizSubmissionRequest model)
        {
            var result = await _submissionService.CreateQuizSubmissionAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("exam")]
        public async Task<IActionResult> CreateExamSubmissionAsync([FromForm] CreateExamSubmissionRequest model)
        {
            var result = await _submissionService.CreateExamSubmissionAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("get-file")]
        public async Task<IActionResult> GetFileAsync([FromQuery] SubmissionSearch search)
        {
            var result = await _submissionService.GetFileAsync(search);
            var fileName = $"submission-{DateTime.Now:yyyyMMddHHmmss}.zip";

            return File(result, "application/zip", fileName);
        }

        [HttpGet()]
        public async Task<IActionResult> GetAsync([FromQuery] SubmissionSearch search)
        {
            var result = await _submissionService.GetAllBySearch(search);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("grade")]
        public async Task<IActionResult> GradeAsync([FromBody] GradeRequestModel model)
        {
            var result = await _submissionService.GradeAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
