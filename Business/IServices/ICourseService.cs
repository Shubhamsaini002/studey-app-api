using studyapp.Models;

namespace studyapp.Business.IServices
{
    public interface ICourseService
    {
        Task<ResponseVM> getCourses(int userId);
        Task<ResponseVM> sescribecourses(int userId,int courseId);
        Task<ResponseVM> GetQuestions(int userId,int courseId,string level);
        Task<ResponseVM> MarkAsRead(int userId, int questionId);
        Task<ResponseVM> SaveQuestion(int userId, int questionId);

        Task<ResponseVM> insertquestions(InsertQuestion data);

    }
}
