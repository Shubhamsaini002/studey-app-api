using Microsoft.EntityFrameworkCore;
using studyapp.Business.IServices;
using studyapp.Data;
using studyapp.Models;

namespace studyapp.Business.Services
{
    public class TestsService: ITestsService
    {
       private readonly AppDbContext _context;
        
        public TestsService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseVM> Dashboard(int userId)
        {
            var totalTests = await _context.Tests
        .Where(x => x.IsActive)
        .CountAsync();

            // 🔹 User attempts (include relations)
            var userTests = await _context.UserTests
                .Where(x => x.UserId == userId)
                .Include(x => x.Test)
                .ThenInclude(t => t.Course)
                .ToListAsync();

            // 🔥 BEST attempt per test (IMPORTANT)
            var bestAttempts = userTests
                .GroupBy(x => x.TestId)
                .Select(g => g.OrderByDescending(x => x.Score).First())
                .ToList();

            int completedTests = bestAttempts.Count;
            int pendingTests = totalTests - completedTests;

            // 🔹 Score calculations
            int totalQuestions = bestAttempts.Sum(x => x.TotalQuestions);
            int totalCorrect = bestAttempts.Sum(x => x.Score);

            int accuracy = totalQuestions == 0 ? 0 :
                (int)Math.Round((double)totalCorrect / totalQuestions * 100);

            int avgScore = completedTests == 0 ? 0 :
                (int)Math.Round(bestAttempts.Average(x => x.Score));

            int bestScore = completedTests == 0 ? 0 :
                bestAttempts.Max(x => x.Score);

            // 🔥 Course performance
            var courseStats = bestAttempts
                .GroupBy(x => x.Test.Course.Name)
                .Select(g => new
                {
                    courseName = g.Key,
                    avgScore = (int)Math.Round(g.Average(x => x.Score))
                })
                .OrderByDescending(x => x.avgScore)
                .ToList();

            // 🔥 Weak course (lowest avg)
            var weakCourse = courseStats
                .OrderBy(x => x.avgScore)
                .FirstOrDefault();

            // 🔥 Recent activity (latest attempts, NOT best)
            var recent = userTests
                .OrderByDescending(x => x.CompletedAt)
                .Take(5)
                .Select(x => new
                {
                    testName = x.Test.Title,
                    score = x.Score,
                    total = x.TotalQuestions
                })
                .ToList();

            return new ResponseVM()
            {
                status = 1,
                data = new
                {
                    totalTests,
                    completedTests,
                    pendingTests,
                    avgScore,
                    bestScore,
                    accuracy,
                    weakCourse,
                    courseStats,
                    recent
                }
            };
        }

        public async Task<ResponseVM> GetAllTest(int userId)
        {
            var result = await _context.Tests
         .Where(t => t.IsActive)
         .Select(t => new
         {
             testId = t.Id,
             title = t.Title,
             description = t.Description,
             totalQuestions = t.TotalQuestions,
             duration = t.DurationMinutes,

             courseId = t.CourseId,
             courseName = t.Course.Name,     // ✅ works because of relation
             courseImage = t.Course.Image,   // optional for UI

             userTest = _context.UserTests
                 .Where(ut => ut.UserId == userId && ut.TestId == t.Id)
                 .Select(ut => new
                 {
                     ut.Id,
                     ut.Status,
                     ut.Score,
                     ut.StartedAt,
                     ut.CompletedAt
                 })
                 .FirstOrDefault()
         })
         .ToListAsync();

            var final = result.Select(x => new 
            {
                TestId = x.testId,
                Title = x.title,
                Description = x.description,
                TotalQuestions = x.totalQuestions,
                Duration = x.duration,

                CourseId = x.courseId,
                CourseName = x.courseName,

                UserTestId = x.userTest?.Id,
                Status = x.userTest?.Status ?? "pending",
                Score = x.userTest?.Score ?? 0,

                startedAt = x.userTest?.StartedAt,
                completedAt = x.userTest?.CompletedAt
            }).ToList();

            return new ResponseVM()
            {
                status = 1,
                data =  final
    .OrderBy(x => x.Status == "completed")
    .ThenByDescending(x => x.startedAt)   
    .ToList()
            };
        }

        public async Task<ResponseVM> GetTestQuestions(int testId)
        {
            var questions = await _context.TestQuestions
               .Where(q => q.TestId == testId)
               .Select(q => new
               {
                   questionId = q.Id,
                   questionText = q.QuestionText,
                   optionA = q.OptionA,
                   optionB = q.OptionB,
                   optionC = q.OptionC,
                   optionD = q.OptionD
               })
                .ToListAsync();


            return new ResponseVM()
            {
                data = questions
            };
        }

        public async Task<ResponseVM> SubmitTest(SubmitTestRequest request)
        {
            var questions = await _context.TestQuestions
                .Where(q => q.TestId == request.TestId)
                .ToListAsync();


            int score = 0;
            var userAnswers = new List<UserAnswer>();

            foreach (var ans in request.Answers)
            {
                var question = questions.FirstOrDefault(q => q.Id == ans.QuestionId);
                if (question == null) continue;

                bool isCorrect = question.CorrectAnswer == ans.SelectedAnswer;
                if (isCorrect) score++;

                userAnswers.Add(new UserAnswer
                {
                    QuestionId = ans.QuestionId,
                    SelectedAnswer = ans.SelectedAnswer,
                    IsCorrect = isCorrect
                });
            }

            // 🔥 STEP 1: Check existing attempt
            var userTest = await _context.UserTests
                .FirstOrDefaultAsync(x => x.UserId == request.UserId && x.TestId == request.TestId);

            if (userTest != null)
            {
                // ✅ UPDATE existing record
                userTest.Score = score;
                userTest.TotalQuestions = questions.Count;
                userTest.Status = "completed";
                userTest.CompletedAt = DateTime.UtcNow;

                // 🔥 Remove old answers
                var oldAnswers = _context.UserAnswers
                    .Where(x => x.UserTestId == userTest.Id);

                _context.UserAnswers.RemoveRange(oldAnswers);

                // Save changes before adding new answers
                await _context.SaveChangesAsync();
            }
            else
            {
                // ✅ CREATE new record
                userTest = new UserTest
                {
                    UserId = request.UserId,
                    TestId = request.TestId,
                    Score = score,
                    TotalQuestions = questions.Count,
                    Status = "completed",
                    StartedAt = DateTime.UtcNow,
                    CompletedAt = DateTime.UtcNow
                };

                await _context.UserTests.AddAsync(userTest);
                await _context.SaveChangesAsync();
            }

            // 🔥 STEP 2: Add new answers
            foreach (var ans in userAnswers)
            {
                ans.UserTestId = userTest.Id;
            }

            await _context.UserAnswers.AddRangeAsync(userAnswers);
            await _context.SaveChangesAsync();

            return new ResponseVM()
            {
                status = 1,
                data = new
                {
                    score = score,
                    total = questions.Count
                }
            };
        }
    }
}
