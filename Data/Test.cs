namespace studyapp.Data
{
    public class Test
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public int TotalQuestions { get; set; }
        public int DurationMinutes { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public List<TestQuestion> Questions { get; set; }
        public Course Course { get; set; }
    }
}
