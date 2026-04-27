using studyapp.Models;

namespace studyapp.Business.IServices
{
    public interface ITestsService
    {
        Task<ResponseVM> GetAllTest(int userId);
        Task<ResponseVM> GetTestQuestions(int testId);
        Task<ResponseVM> SubmitTest(SubmitTestRequest request);
    }
}
