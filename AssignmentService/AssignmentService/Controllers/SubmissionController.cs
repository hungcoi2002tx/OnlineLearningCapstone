using AssignmentService.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> CreateQuizSubmission([FromBody] CreateQuizSubmissionRequest model)
        {
            var result = await _submissionService.CreateQuizSubmissionAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
