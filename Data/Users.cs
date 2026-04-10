namespace studyapp.Data
{
    public class User
    {
        public int Id { get; set; } // Auto increment

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public DateTime? LastLoginTime { get; set; }

        public DateTime CreatedTime { get; set; }

        public bool IsActive { get; set; }

        public string VerificationCode { get; set; }
        public string JobTitle { get; set; }
        public bool IsReview { get; set; }
    }
}
