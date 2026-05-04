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


        [HttpGet("GethimalayasJobs")]
        public async Task<IActionResult> GethimalayasJobs()
        {
            try
            {
                var clientIp =
                    Request.Headers["X-Forwarded-For"].FirstOrDefault()?.Split(',').FirstOrDefault()
                    ?? HttpContext.Connection.RemoteIpAddress?.ToString();

                if (string.IsNullOrWhiteSpace(clientIp) ||
                    clientIp == "::1" ||
                    clientIp == "127.0.0.1")
                {
                    clientIp = "49.45.134.10";
                }

                string countryName = "India";

                try
                {
                    var geoResponse = await _httpClient.GetAsync(
                        $"https://api.db-ip.com/v2/free/{clientIp}");

                    if (geoResponse.IsSuccessStatusCode)
                    {
                        var geoJson = await geoResponse.Content.ReadAsStringAsync();

                        using var geoDoc = JsonDocument.Parse(geoJson);

                        countryName = geoDoc.RootElement
                            .GetProperty("countryName")
                            .GetString() ?? "India";
                    }
                }
                catch
                {
                    countryName = "India";
                }

                var jobsResponse = await _httpClient.GetAsync(
                    $"https://himalayas.app/jobs/api/search?country={countryName}");

                if (!jobsResponse.IsSuccessStatusCode)
                {
                    return BadRequest("Jobs API failed");
                }

                var jobsJson = await jobsResponse.Content.ReadAsStringAsync();

                using var jobsDoc = JsonDocument.Parse(jobsJson);

                // FIX: jobs is directly an array
                var jobsArray = jobsDoc.RootElement.GetProperty("jobs");

                var jobsList = jobsArray
                    .EnumerateArray()
                    .Select(job => new
                    {
                        Title = job.GetProperty("title").GetString(),
                        CompanyName = job.GetProperty("companyName").GetString(),
                        CompanyLogo = job.GetProperty("companyLogo").GetString(),
                        EmploymentType = job.GetProperty("employmentType").GetString(),

                        MinSalary = job.TryGetProperty("minSalary", out var minSalary)
                            ? minSalary.ToString()
                            : null,

                        MaxSalary = job.TryGetProperty("maxSalary", out var maxSalary)
                            ? maxSalary.ToString()
                            : null,

                        Seniority = job.TryGetProperty("seniority", out var seniority)
                            ? seniority.EnumerateArray()
                                .Select(x => x.GetString())
                                .ToList()
                            : new List<string>(),

                        Currency = job.GetProperty("currency").GetString(),

                        LocationRestrictions = job.TryGetProperty("locationRestrictions", out var location)
                            ? location.EnumerateArray()
                                .Select(x => x.GetString())
                                .ToList()
                            : new List<string>(),

                        Categories = job.TryGetProperty("categories", out var categories)
                            ? categories.EnumerateArray()
                                .Select(x => x.GetString()?
                                    .Replace("_", " ")
                                    .Replace("-", " "))
                                .ToList()
                            : new List<string>(),

                        Description = job.GetProperty("description").GetString(),

                        ApplicationLink = job.GetProperty("applicationLink").GetString()
                    })
                    .ToList();

                return Ok(new
                {
                    Jobs = jobsList
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
