using Microsoft.EntityFrameworkCore;
using studyapp.Business.IServices;
using studyapp.Data;
using studyapp.Models;

namespace studyapp.Business.Services
{
    public class JobsService : IJobsservices
    {
        private readonly AppDbContext _context;

        public JobsService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ResponseVM> GetJobs()
        {
            var jobs = await _context.Jobs
                    .OrderByDescending(x => x.CreatedAt)
                    .ToListAsync();

            return new ResponseVM
            {
                status = 1,
                Message = "Jobs fetched successfully",
                data = jobs
            };
        }
    }
}
