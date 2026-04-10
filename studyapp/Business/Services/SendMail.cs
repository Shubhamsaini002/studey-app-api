using studyapp.Business.IServices;
using studyapp.Models;
using MailKit.Net.Smtp;
using MimeKit;
namespace studyapp.Business.Services
{
    public class SendMail: ISendMail
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;

        public SendMail(IConfiguration config, IWebHostEnvironment env)
        {
            _config = config;
            _env = env;
        }
        public async Task<SendOtpResponse> SendOtpEmailAsync(string toEmail, string subject,string otp,bool type)
        {
            try
            {
                string template = type ? "otp-template.html" : "restpassword.html";
                var templatePath = Path.Combine(AppContext.BaseDirectory, "Templates", template);
                if (!File.Exists(templatePath))
                    throw new Exception("Template not found: " + templatePath);

                var htmlBody = await File.ReadAllTextAsync(templatePath);

                htmlBody = htmlBody.Replace("{{OTP}}", otp);

                var email = new MimeMessage();
                email.From.Add(new MailboxAddress("HireMint", _config["EmailSettings:Email"]));
                email.To.Add(MailboxAddress.Parse(toEmail));
                email.Subject = subject;

                email.Body = new BodyBuilder
                {
                    HtmlBody = htmlBody
                }.ToMessageBody();

                using var smtp = new SmtpClient();
                smtp.CheckCertificateRevocation = false;
                await smtp.ConnectAsync(
                    "smtp.gmail.com",
                    587,
                    MailKit.Security.SecureSocketOptions.StartTls
                );

                await smtp.AuthenticateAsync(
                    _config["EmailSettings:Email"],
                    _config["EmailSettings:Password"]
                );

                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);

                return new SendOtpResponse
                {
                    Success = true,
                    Message = "OTP sent successfully"
                };
            }
            catch (Exception ex)
            {
                return new SendOtpResponse
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}
