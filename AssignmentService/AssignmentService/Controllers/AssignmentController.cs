using AssignmentService.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Share.DTO;
using Share.RequestModel;

namespace AssignmentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentController : ControllerBase
    {
        readonly IAssigmentService _assignmentService;

        public AssignmentController(IAssigmentService assignmentService)
        {
            _assignmentService = assignmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return Ok("Hello from Assignment Service");
        }

        [HttpPost]
        public async Task<IActionResult> CreateAssignmentAsync([FromForm] ExamRequestModel exam)
        {
            var result = await _assignmentService.CreateExamAsync(exam);
            return StatusCode((int)result.StatusCode, result);
        }

        
    }
}
