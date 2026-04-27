using Microsoft.AspNetCore.Mvc;
using studyapp.Business.IServices;
using studyapp.Models;

namespace studyapp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserTestsController : Controller
    {
        private readonly ITestsService _testService;

        public UserTestsController(ITestsService testsService) {
            _testService = testsService;

        }

        [HttpGet("getalltest")]
        public async Task<IActionResult> GetAlltest(int userId)
        {
            try
            {
                var result = await _testService.GetAllTest(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetTestQuestions")]
        public async Task<IActionResult> GetTestQuestions(int testId)
        {
            try
            {
                var result = await _testService.GetTestQuestions(testId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("SubmitTest")]
        public async Task<IActionResult> SubmitTest(SubmitTestRequest request)
        {
            try
            {
                var result = await _testService.SubmitTest(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
