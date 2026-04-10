using studyapp.Models;

namespace studyapp.Business.IServices
{
    public interface IAdminService
    {
        Task<ResponseVM> database(adminVM data);
    }
}
