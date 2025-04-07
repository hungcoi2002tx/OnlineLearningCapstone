using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AssignmentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssigmentController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return Ok("Hello from Assignment Service");
        }
    }
}
