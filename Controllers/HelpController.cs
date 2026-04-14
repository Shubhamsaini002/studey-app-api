using Microsoft.AspNetCore.Mvc;
using studyapp.Data;

namespace studyapp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HelpController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HelpController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("help")]
        public async Task<IActionResult> SubmitHelpRequest([FromBody] HelpRequest model)
        {
            if (model == null)
                return BadRequest("Invalid data");

            model.CreatedAt = DateTime.Now;
            model.Status = "Pending";

            _context.HelpRequests.Add(model);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Request submitted successfully" });
        }
    }
}
