using Microsoft.AspNetCore.Mvc;
using studyapp.Business.IServices;
using studyapp.Models;

namespace studyapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet("getcourses")]
        public async Task<IActionResult> GetCourses(int userId)
        {
            var result = await _courseService.getCourses(userId);
            return Ok(result);
        }

        [HttpGet("sescribecourses")]
        public async Task<IActionResult> sescribecourses(int userId,int courseId)
        {
            var result = await _courseService.sescribecourses(userId, courseId);
            return Ok(result);
        }

        [HttpGet("getquestions")]
        public async Task<IActionResult> GetQuestions(
         int userId,
         int courseId,
         string? level)
        {
            var result = await _courseService.GetQuestions(userId, courseId, level);

            return Ok(result);
        }

        // ✅ 2. Mark as Read
        [HttpGet("markread")]
        public async Task<IActionResult> MarkAsRead(int userId, int questionId)
        {
            var result=await _courseService.MarkAsRead(userId, questionId);

            return Ok(result);
        }

        // ✅ 3. Toggle Save
        [HttpGet("savequestion")]
        public async Task<IActionResult> SaveQuestion(int userId, int questionId)
        {
            var result= await _courseService.SaveQuestion(userId, questionId);

            return Ok(result);
        }

        // ✅ 4. Dashboard API
        //[HttpGet("dashboard")]
        //public async Task<IActionResult> GetDashboard(int userId)
        //{
        //    var total = await _context.Questions.CountAsync();

        //    var read = await _context.UserQuestionStatuses
        //        .CountAsync(x => x.UserId == userId && x.IsRead);

        //    var saved = await _context.UserQuestionStatuses
        //        .CountAsync(x => x.UserId == userId && x.IsSaved);

        //    var progress = total == 0 ? 0 : (read * 100) / total;

        //    return Ok(new
        //    {
        //        TotalQuestions = total,
        //        ReadQuestions = read,
        //        SavedQuestions = saved,
        //        Progress = progress
        //    });
        //}

        [HttpPost("insertquestions")]
        public async Task<IActionResult> insertquestions(InsertQuestion data)
        {
            var result = await _courseService.insertquestions(data);

            return Ok(result);
        }
    }
}
