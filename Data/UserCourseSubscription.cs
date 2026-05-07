namespace studyapp.Data
{
    public class UserCourseSubscription
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int CourseId { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime SubscribedAt { get; set; } = DateTime.UtcNow;

        public Course? Course { get; set; }
    }
}
