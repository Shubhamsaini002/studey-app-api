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

        [HttpGet("login")]
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

        [HttpGet("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(string email, string code, string password)
        {
            try
            {
                var result = await _userservice.ForgetPassword( email,  code, password);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("OtpVerification")]
        public async Task<IActionResult> OtpVerification(string Email,string code)
        {
            try
            {
                var result = await _userservice.OtpVerification( Email, code);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("ResendOTP")]
        public async Task<IActionResult> ResendOTP(SendOtpRequest data)
        {
            try
            {
                var result = await _userservice.SendOTP(data.Email, data.type);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Notification")]
        public async Task<IActionResult> Notification()
        {
            try
            {
                var result = await _userservice.Notification();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
