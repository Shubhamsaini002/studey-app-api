using Microsoft.AspNetCore.Mvc;
using studyapp.Business.IServices;
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

        [HttpPost("login")]
        public async Task<IActionResult> login(string Email, string Password)
        {
            try
            {
                var result = await _userservice.Login(Email,Password);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(string Mail, string code, string password)
        {
            try
            {
                var result = await _userservice.ForgetPassword( Mail,  code, password);
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

        [HttpGet("ResendOTP")]
        public async Task<IActionResult> ResendOTP(string Mail)
        {
            try
            {
                var result = await _userservice.SendOTP(Mail);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var result = await _userservice.GetAllUsers();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
