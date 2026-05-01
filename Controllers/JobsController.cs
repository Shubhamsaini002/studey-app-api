using Microsoft.AspNetCore.Mvc;
using studyapp.Business.IServices;
using studyapp.Business.Services;

namespace studyapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : Controller
    {
        private readonly IJobsservices _jobsservices;
        public JobsController(IJobsservices jobsservices) {
            _jobsservices = jobsservices;
        }
        [HttpGet("getjobs")]
        public async Task<IActionResult> GetJobs()
        {
            var result = await _jobsservices.GetJobs();
            return Ok(result);
        }
    }
}
