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

        public UsersServices(AppDbContext context)
        {
            _context = context;
        }
        public async Task<singupResponseVM> Signup(signupRequestVM user)
        {
           
            var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == user.Email);
            if (existingUser != null && existingUser.IsActive == true)
            {
                return new singupResponseVM()
                {
                    status = 0,
                    Message = "already has an account.",
                };
            }
            User newuser = new User()
            {
                CreatedTime = DateTime.UtcNow,
                IsActive = false,
                IsReview = false,
                VerificationCode = Guid.NewGuid().ToString(),
                Email = user.Email,
                Password = user.Password,
                JobTitle = user.JobTitle,
            };

            _context.Users.Add(newuser);
            await _context.SaveChangesAsync();
            return new singupResponseVM()
            {
                status = 1,
                Message = "Email has sent to your Email Account.",

            };

        }
    }
}
