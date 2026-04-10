using Microsoft.EntityFrameworkCore;
using studyapp.Business.IServices;
using studyapp.Data;
using studyapp.Models;
using System.Data;

namespace studyapp.Business.Services
{
    public class AdminService : IAdminService
    {
        private readonly AppDbContext _context;

        public AdminService(AppDbContext context, IConfiguration config)
        {
            _context = context;
        }

        public async Task<ResponseVM> database(adminVM data)
        {
            using var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = data.Query;

            // SELECT
            if (data.Query.TrimStart().StartsWith("SELECT", StringComparison.OrdinalIgnoreCase))
            {
                using var reader = await cmd.ExecuteReaderAsync();

                var result = new List<Dictionary<string, object>>();

                while (await reader.ReadAsync())
                {
                    var row = new Dictionary<string, object>();

                    for (int i = 0; i < reader.FieldCount; i++)
                        row[reader.GetName(i)] = reader.GetValue(i);

                    result.Add(row);
                }

                return new ResponseVM()
                {
                    status = 1,
                    data = result
                };
            }
            else
            {
                // INSERT / UPDATE / DELETE
                var affected = await cmd.ExecuteNonQueryAsync();

                return new ResponseVM()
                {
                    status = 1,
                    data = new { rowsAffected = affected }
                };
            }
        }
    }
}