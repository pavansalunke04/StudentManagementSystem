using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;

// ✅ Alias fix
using StudentEntity = StudentManagementSystem.Data.Student;

namespace StudentManagementSystem.Pages.Admin
{
    public class DashboardModel : PageModel
    {
        private readonly AppDbContext _db;
        public DashboardModel(AppDbContext db) { _db = db; }

        public int TotalStudents { get; set; }
        public int TotalTeachers { get; set; }
        public int PresentToday { get; set; }

        // ✅ StudentEntity use karo
        public List<StudentEntity> RecentStudents { get; set; } = new();

        public Dictionary<string, int> CourseAttendance { get; set; } = new();

        public async Task OnGetAsync()
        {
            TotalStudents = await _db.Students.CountAsync();
            TotalTeachers = await _db.Teachers.CountAsync();

            var today = DateTime.UtcNow.Date;

            PresentToday = await _db.Attendances
                .Where(a => a.Date.Date == today && a.Status == "Present")
                .CountAsync();

            // ✅ StudentEntity list
            RecentStudents = await _db.Students
                .OrderByDescending(s => s.Id)
                .Take(5)
                .ToListAsync();
        }
    }
}