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
        public async Task<ResponseVM> getCourses(int userId)
        {
            var result = await _context.Courses
       .Select(course => new
       {
           course.Id,
           course.Name,
           course.Description,
           course.Image,

           IsSubscribed = _context.UserCourseSubscription
               .Any(sub =>
                   sub.UserId == userId &&
                   sub.CourseId == course.Id &&
                   sub.IsActive),

           TotalQuestions = _context.Questions
               .Count(q => q.CourseId == course.Id),

           ReadQuestions = (
               from qs in _context.UserQuestionStatuses
               join q in _context.Questions
                   on qs.QuestionId equals q.Id
               where qs.UserId == userId
                     && qs.IsRead
                     && q.CourseId == course.Id
               select qs.Id
           ).Count()
       })

       // SUBSCRIBED COURSES FIRST
       .OrderByDescending(x => x.IsSubscribed)

       // THEN LATEST COURSES
       .ThenByDescending(x => x.Id)

       .ToListAsync();


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

           where q.CourseId == courseId && q.Level == level
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

        public async Task<ResponseVM> insertquestions(InsertQuestion data)
        {
            Question record = new Question()
            {
                CourseId = data.CourseId,
                QuestionText = data.QuestionText,
                AnswerText = data.AnswerText,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                Level = data.Level,
            };

            try
            {
                _context.Questions.Add(record);
                await _context.SaveChangesAsync();
                return new ResponseVM()
                {
                    status = 1,
                    Message = "added.."
                };
            }
            catch (Exception ex) {
                return new ResponseVM()
                {
                    status = 0,
                    Message = ex.InnerException?.Message ?? ex.Message,
                };
            }
        }

        public async Task<ResponseVM> sescribecourses(int userId, int courseId)
        {
            var courseExists = await _context.Courses
       .AnyAsync(x => x.Id == courseId);

            if (!courseExists)
            {
                return new ResponseVM
                {
                    status = 0,
                    Message = "Course not found"
                };
            }

            var alreadySubscribed = await _context.UserCourseSubscription
                .FirstOrDefaultAsync(x =>
                    x.UserId == userId &&
                    x.CourseId == courseId);

            // If already subscribed and active
            if (alreadySubscribed != null && alreadySubscribed.IsActive)
            {
                return new ResponseVM
                {
                    status = 0,
                    Message = "Already subscribed"
                };
            }

            // If exists but inactive -> reactivate
            if (alreadySubscribed != null)
            {
                alreadySubscribed.IsActive = true;
                alreadySubscribed.SubscribedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return new ResponseVM
                {
                    status = 1,
                    Message = "Course subscribed successfully"
                };
            }

            // New subscription
            var subscription = new UserCourseSubscription
            {
                UserId = userId,
                CourseId = courseId,
                IsActive = true,
                SubscribedAt = DateTime.UtcNow
            };

            _context.UserCourseSubscription.Add(subscription);

            await _context.SaveChangesAsync();

            return new ResponseVM
            {
                status = 1,
                Message = "Course subscribed successfully"
            };
        }
    }
}
