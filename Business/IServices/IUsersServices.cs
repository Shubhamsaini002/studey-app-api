using Microsoft.AspNetCore.Mvc;
using studyapp.Data;
using studyapp.Models;

namespace studyapp.Business.IServices
{
    public interface IUsersServices
    {
        Task<singupResponseVM> Signup(signupRequestVM user);
        Task<ResponseVM> Login(string Email, string Password);

        Task<ResponseVM> OtpVerification(string Mail, string code);

        Task<ResponseVM> ForgetPassword(string Mail, string code, string password);

        Task<ResponseVM> SendOTP(string Mail ,bool type);
        Task<ResponseVM> GetAllUsers();
    }
}
