using Microsoft.AspNetCore.Mvc;
using studyapp.Data;
using studyapp.Models;

namespace studyapp.Business.IServices
{
    public interface IUsersServices
    {
        Task<singupResponseVM> Signup(signupRequestVM user);

    }
}
