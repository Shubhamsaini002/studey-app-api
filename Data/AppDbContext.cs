using Microsoft.EntityFrameworkCore;
namespace studyapp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):
            base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<UserQuestionStatus> UserQuestionStatuses { get; set; }
        public DbSet<HelpRequest> HelpRequests { get; set; }

        public DbSet<Test> Tests { get; set; }
        public DbSet<TestQuestion> TestQuestions { get; set; }
        public DbSet<UserTest> UserTests { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 🔥 Unique constraint (VERY IMPORTANT)
            modelBuilder.Entity<UserQuestionStatus>()
                .HasIndex(x => new { x.UserId, x.QuestionId })
                .IsUnique();

            modelBuilder.Entity<Test>()
           .HasMany(t => t.Questions)
           .WithOne(q => q.Test)
           .HasForeignKey(q => q.TestId);
             modelBuilder.Entity<Test>()
        .HasOne(t => t.Course)
        .WithMany()
        .HasForeignKey(t => t.CourseId);

            modelBuilder.Entity<UserTest>()
                .HasMany(u => u.Answers)
                .WithOne(a => a.UserTest)
                .HasForeignKey(a => a.UserTestId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
