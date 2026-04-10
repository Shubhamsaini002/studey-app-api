using Microsoft.AspNetCore.Mvc;
using studyapp.Business.IServices;
using studyapp.Data;
using studyapp.Models;

namespace studyapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUsersServices _userservice;

        public UsersController(IUsersServices service)
        {
            _userservice = service;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup(signupRequestVM user)
        {
            try
            {
                var result = await _userservice.Signup(user);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("OtpVerification")]
        public async Task<IActionResult> OtpVerification(string Mail,string code)
        {
            try
            {
                var result = await _userservice.OtpVerification( Mail, code);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
