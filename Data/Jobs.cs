using System.Data;

namespace studyapp.Data
{
    public class Jobs
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string? CompanyName { get; set; }
        public string CompanyLogo { get; set; }
        public string? RecruiterName { get; set; }
        public string? RecruiterEmail { get; set; }
        public string? RecruiterNumber { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Technology { get; set; }
        public string? Role { get; set; }
        public string? Description { get; set; }
        public string? Link { get; set; }
        public bool Status { get; set; } = false;
        public DateTime CreatedAt { get; set; }= DateTime.Now;
    }
}
