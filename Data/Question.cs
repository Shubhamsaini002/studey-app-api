namespace studyapp.Data
{
    public class Question
    {
        public int Id { get; set; }

        public int CourseId { get; set; }
        public string Level { get; set; } // basic / intermediate / advanced

        public string QuestionText { get; set; }
        public string AnswerText { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Course Course { get; set; }
    }
}
