namespace studyapp.Data
{
    public class UserAnswer
    {
        public int Id { get; set; }

        public int UserTestId { get; set; }
        public int QuestionId { get; set; }

        public string SelectedAnswer { get; set; }

        public bool IsCorrect { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public UserTest UserTest { get; set; }
    }
}
