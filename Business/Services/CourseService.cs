using Microsoft.EntityFrameworkCore;
using studyapp.Business.IServices;
using studyapp.Data;
using studyapp.Models;

namespace studyapp.Business.Services
{
    public class CourseService : ICourseService
    {
        private readonly AppDbContext _context;

        public CourseService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ResponseVM> getCourses()
        {
            var result = await _context.Courses.ToListAsync();
            return new ResponseVM
            {
                status = 1,
                data = result
            };
        }

        public async Task<ResponseVM> GetQuestions(int userId, int courseId, string level)
        {
            var result = await (
           from q in _context.Questions
         join uqs in _context.UserQuestionStatuses
           on new { QId = q.Id, UId = userId }
           equals new { QId = uqs.QuestionId, UId = uqs.UserId }
           into gj
           from sub in gj.DefaultIfEmpty()

           where q.CourseId == courseId

       select new
       {
           q.Id,
           Question = q.QuestionText,
           Answer = q.AnswerText,
           q.Level,
           IsRead = sub != null && sub.IsRead,
           IsSaved = sub != null && sub.IsSaved
       }).ToListAsync();

            return new ResponseVM
            {
                status = 1,
                data= result
            };

        }

        public async Task<ResponseVM> MarkAsRead(int userId, int questionId)
        {
            var item = await _context.UserQuestionStatuses
                .FirstOrDefaultAsync(x => x.UserId == userId && x.QuestionId == questionId);

            if (item == null)
            {
                _context.UserQuestionStatuses.Add(new UserQuestionStatus
                {
                    UserId = userId,
                    QuestionId = questionId,
                    IsRead = true,
                    UpdatedAt = DateTime.UtcNow
                });
            }
            else
            {
                item.IsRead = true;
                item.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return new ResponseVM()
            {
                status = 1,
                Message="Marked as Saved."
            };
        }

        public async Task<ResponseVM> SaveQuestion(int userId, int questionId)
        {
            var item = await _context.UserQuestionStatuses
                 .FirstOrDefaultAsync(x => x.UserId == userId && x.QuestionId == questionId);

            if (item == null)
            {
                _context.UserQuestionStatuses.Add(new UserQuestionStatus
                {
                    UserId = userId,
                    QuestionId = questionId,
                    IsSaved = true,
                    UpdatedAt = DateTime.UtcNow
                });
            }
            else
            {
                item.IsSaved = !item.IsSaved;
                item.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return new ResponseVM
            {
                status = 1,
                Message = "Saved."
            };
        }
    }
}
