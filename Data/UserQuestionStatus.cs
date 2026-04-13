namespace studyapp.Data
{
    public class UserQuestionStatus
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public int QuestionId { get; set; }

        public bool IsRead { get; set; } = false;
        public bool IsSaved { get; set; } = false;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
