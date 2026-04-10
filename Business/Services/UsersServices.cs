using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using studyapp.Business.IServices;
using studyapp.Data;
using studyapp.Models;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
using static System.Net.WebRequestMethods;

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
                Message = "OTP has sent to your Email Account.",

            };

        }

        public async Task<ResponseVM> Login(string Email, string Password)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == Email && x.Password == Password && x.IsActive==true);
            if (existingUser == null)
            {
                return new ResponseVM()
                {
                    status = 0,
                    Message = "Please Check Your Email Or Password!",
                };
            }

            existingUser.LastLoginTime = DateTime.UtcNow;
            _context.Users.Update(existingUser);
            await _context.SaveChangesAsync();
            return new ResponseVM()
            {
                status = 1,
                Message = "Verified successfully",
                data = new UserInfoVM()
                {
                    Id = existingUser.Id,
                    Name = existingUser.Name,
                    Email = existingUser.Email,
                    JobTitle = existingUser.JobTitle,
                    IsActive = true,
                    IsReview = existingUser.IsReview,
                }
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
            
            existingUser.IsActive =true;
            _context.Users.Update(existingUser);
            await _context.SaveChangesAsync();
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
                    IsActive=true,
                    IsReview=existingUser.IsReview,
                }
            };
        }

        public async Task<ResponseVM> ForgetPassword(string Mail, string code, string password)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == Mail && x.VerificationCode == code);
            if (existingUser == null)
            {
                return new ResponseVM()
                {
                    status = 0,
                    Message = "Invalid OTP!",
                };
            }

            existingUser.Password = password;
            _context.Users.Update(existingUser);
            await _context.SaveChangesAsync();
            return new ResponseVM()
            {
                status = 1,
                Message = "New Password Genrated!",
                data = new UserInfoVM()
                {
                    Id = existingUser.Id,
                    Name = existingUser.Name,
                    Email = existingUser.Email,
                    JobTitle = existingUser.JobTitle,
                    IsActive = true,
                    IsReview = existingUser.IsReview,
                }
            };
        }


        public async Task<ResponseVM> SendOTP(string Mail)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == Mail);
            if (existingUser == null)
            {
                return new ResponseVM()
                {
                    status = 0,
                    Message = "Account does not exist.",
                };
            }
            var random = new Random();
            int otp = random.Next(100000, 1000000);
            existingUser.VerificationCode = otp.ToString();
            _context.Users.Update(existingUser);
            await _context.SaveChangesAsync();
            return new ResponseVM()
            {
                status = 0,
                Message = "OTP has sent to your Email Account.",
            };

        }

        public async Task<ResponseVM> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();
            return new ResponseVM()
            {
                status = 1,
                data = users
            };

        }
    }
}
