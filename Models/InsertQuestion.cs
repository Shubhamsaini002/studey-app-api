using studyapp.Data;

namespace studyapp.Models
{
    public class InsertQuestion
    {

        public int CourseId { get; set; }
        public string Level { get; set; } // basic / intermediate / advanced

        public string QuestionText { get; set; }
        public string AnswerText { get; set; }

    }

    public class InsertQuestionsBulkData
    {
        public List<InsertQuestion> Questions { get; set; }
    }
}
