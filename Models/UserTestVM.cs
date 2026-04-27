namespace studyapp.Models
{
    public class SubmitTestRequest
    {
        public int UserId { get; set; }
        public int TestId { get; set; }

        public List<AnswerDto> Answers { get; set; }
    }

    public class AnswerDto
    {
        public int QuestionId { get; set; }
        public string SelectedAnswer { get; set; }
    }
}
