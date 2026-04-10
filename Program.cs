using Microsoft.EntityFrameworkCore;
using studyapp.Business.IServices;
using studyapp.Business.Services;
using studyapp.Data;

var builder = WebApplication.CreateBuilder(args);

// Detect environment
var isDevelopment = builder.Environment.IsDevelopment();

// SQLite path (different for dev vs prod)
var dbPath = isDevelopment
    ? "studyapp.db"                    // local file
    : "/home/data/studyapp.db";        // Azure persistent storage

// Ensure directory exists in production
if (!isDevelopment)
{
    Directory.CreateDirectory("/home/data");
}

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// DI Services
builder.Services.AddScoped<IUsersServices, UsersServices>();
builder.Services.AddScoped<ISendMail, SendMail>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// CORS
app.UseCors("AllowAll");

// Swagger (DEV + PROD)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "StudyApp API V1");
    c.RoutePrefix = "swagger";
});

// Auto migrate database
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();