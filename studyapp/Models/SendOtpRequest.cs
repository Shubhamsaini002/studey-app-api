namespace studyapp.Models
{
    public class SendOtpRequest
    {
        public bool type { get; set; }
        public string Email { get; set; }
        public string tocken { get; set; }
    }
}
