using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using studyapp.Business.IServices;
using studyapp.Data;
using studyapp.Models;

namespace studyapp.Business.Services
{
    public class UsersServices : IUsersServices
    {
        private readonly AppDbContext _context;
        private readonly ISendMail _sendMail;

        public UsersServices(AppDbContext context,ISendMail sendMail)
        {
            _context = context;
            _sendMail = sendMail;
        }

        public async Task<singupResponseVM> Signup(signupRequestVM user)
        {
            var random = new Random();
            int otp = random.Next(100000, 1000000);
            var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == user.Email);
            if (existingUser != null )
            { if(existingUser.IsActive == true)
                {
                    return new singupResponseVM()
                    {
                        status = 0,
                        Message = "already has an account.",
                    };
                }
               
                existingUser.Name = user.Name;
                existingUser.VerificationCode = otp.ToString();
                existingUser.Password = user.Password;
                _context.Users.Update(existingUser);
                await _context.SaveChangesAsync();
            }
           
            if (existingUser == null)
            {
                User newuser = new User()
                {
                    Name = user.Name,
                    CreatedTime = DateTime.UtcNow,
                    IsActive = false,
                    IsReview = false,
                    VerificationCode = otp.ToString(),
                    Email = user.Email,
                    Password = user.Password,
                    JobTitle = user.JobTitle,
                };
       
                    _context.Users.Add(newuser);
                await _context.SaveChangesAsync();
            }
            var res = await _sendMail.SendOtpEmailAsync(user.Email, "Verification Code", otp.ToString(),true);
            return new singupResponseVM()
            {
                status = 1,
                Message = "Email has sent to your Email Account.",

            };

        }

        public async Task<ResponseVM> OtpVerification(string Mail, string code)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == Mail && x.VerificationCode==code);
            if (existingUser == null) {
                return new ResponseVM()
                {
                    status = 0,
                    Message = "Invalid OTP!",
                };
            }
            return new ResponseVM()
            {
                status = 1,
                Message = "Verified successfully",
                data=new UserInfoVM()
                {
                    Id=existingUser.Id,
                    Name=existingUser.Name,
                    Email=existingUser.Email,
                    JobTitle=existingUser.JobTitle,
                    IsActive=existingUser.IsActive,
                    IsReview=existingUser.IsReview,
                }
            };
        }
    }
}
