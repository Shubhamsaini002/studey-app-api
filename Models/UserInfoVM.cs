namespace studyapp.Models
{
    public class UserInfoVM
    {
        public int Id { get; set; } 

        public string Name { get; set; }

        public string Email { get; set; }

        public bool IsActive { get; set; }

        public string JobTitle { get; set; }

        public bool IsReview { get; set; }
    }
}
