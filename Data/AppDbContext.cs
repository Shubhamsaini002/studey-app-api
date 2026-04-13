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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 🔥 Unique constraint (VERY IMPORTANT)
            modelBuilder.Entity<UserQuestionStatus>()
                .HasIndex(x => new { x.UserId, x.QuestionId })
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
