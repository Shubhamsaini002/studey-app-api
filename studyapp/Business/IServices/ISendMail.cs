using studyapp.Models;

namespace studyapp.Business.IServices
{
    public interface ISendMail
    {
        Task<SendOtpResponse> SendOtpEmailAsync(string toEmail, string subject, string otp, bool type);
       
    }
}
