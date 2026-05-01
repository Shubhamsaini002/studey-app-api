using studyapp.Models;
namespace studyapp.Business.IServices
{
    public interface IJobsservices
    {
        Task<ResponseVM> GetJobs();
    }
}
