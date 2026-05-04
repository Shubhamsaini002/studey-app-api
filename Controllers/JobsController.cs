using Microsoft.AspNetCore.Mvc;
using studyapp.Business.IServices;
using studyapp.Business.Services;
using System.Globalization;
using System.Text.Json;

namespace studyapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : Controller
    {
        private readonly IJobsservices _jobsservices;
        private readonly HttpClient _httpClient;
        public JobsController(IJobsservices jobsservices, HttpClient httpClient )
        {
            _jobsservices = jobsservices;
            _httpClient = httpClient;
        }
        [HttpGet("getjobs")]
        public async Task<IActionResult> GetJobs()
        {
            var result = await _jobsservices.GetJobs();
            return Ok(result);
        }


      
    }
}
