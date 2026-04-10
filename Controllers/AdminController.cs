using Microsoft.AspNetCore.Mvc;
using studyapp.Business.IServices;
using studyapp.Models;

namespace studyapp.Controllers
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService service)
        {
            _adminService = service;
        }
        [HttpPost("database")]
        public async Task<IActionResult> Database(adminVM data)
        {
            try
            { 
                if( data.Password != "Aa@12345")
                {
                    return BadRequest("Dont have Access...");
                }
                var result = await _adminService.database(data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
