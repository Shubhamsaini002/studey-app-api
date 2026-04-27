namespace studyapp.Data
{
    public class UserTest
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public int TestId { get; set; }

        public int Score { get; set; } = 0;
        public int TotalQuestions { get; set; }

        public string Status { get; set; } // pending / completed

        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public Test Test { get; set; }
        public List<UserAnswer> Answers { get; set; }
    }
}
