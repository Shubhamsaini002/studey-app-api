using studyapp.Models;

namespace studyapp.Business.IServices
{
    public interface ICourseService
    {
        Task<ResponseVM> getCourses();

        Task<ResponseVM> GetQuestions(int userId,int courseId,string level);
        Task<ResponseVM> MarkAsRead(int userId, int questionId);
        Task<ResponseVM> SaveQuestion(int userId, int questionId);

    }
}
