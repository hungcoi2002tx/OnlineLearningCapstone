using AssignmentService.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Share.RequestModel;

namespace AssignmentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        readonly IQuestionService _questionService;

        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [HttpPost("{assignmentId}")]
        public async Task<IActionResult> CreateAsync(string assignmentId, [FromBody] CreateQuestionRequestModel question)
        {
            var result = await _questionService.CreateAsync(assignmentId, question);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateQuestionRequestModel model)
        {
            var result = await _questionService.UpdateAsync(id, model);

            return StatusCode((int)result.StatusCode, result);
        }
    }
}
