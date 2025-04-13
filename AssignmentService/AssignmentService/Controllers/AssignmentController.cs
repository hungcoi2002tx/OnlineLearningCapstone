using AssignmentService.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Share.DTO;
using Share.Other.SearchModel;
using Share.RequestModel;

namespace AssignmentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentController : ControllerBase
    {
        readonly IAssignmentService _assignmentService;

        public AssignmentController(IAssignmentService assignmentService)
        {
            _assignmentService = assignmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] AssignmentSearch model)
        {
            var result = await _assignmentService.GetAllByFilterAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        [Route("create-exam")]
        public async Task<IActionResult> CreateAssignmentAsync([FromForm] ExamRequestModel exam)
        {
            var result = await _assignmentService.CreateExamAsync(exam);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        [Route("create-quiz")]
        public async Task<IActionResult> CreateQuizAssignmentAsync([FromBody] QuizRequestDto exam)
        {
            var result = await _assignmentService.CreateQuizAsync(exam);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id, [FromQuery] string teacherId)
        {
            var result = await _assignmentService.DeleteAsync(id, teacherId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(string id, [FromQuery] string teacherId, [FromBody] UpdateAssignmentRequestModel updateModel)
        {
            var result = await _assignmentService.UpdateAsync(id,teacherId, updateModel);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExamAsync(string id, [FromQuery] string teacherId, [FromBody] UpdateExamRequestModel updateModel)
        {
            var result = await _assignmentService.UpdateAsync(id, teacherId, updateModel);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
