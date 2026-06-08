using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;

namespace StudentManagementSystem.Pages.Student
{
    public class AttendanceModel : PageModel
    {
        private readonly AppDbContext _db;

        public AttendanceModel(AppDbContext db)
        {
            _db = db;
        }

        public int TotalPresent { get; set; }
        public int TotalAbsent { get; set; }
        public string OverallRate { get; set; } = "0%";
        public List<Attendance> RecentRecords { get; set; } = new();

        public async Task OnGetAsync()
        {
            // StudentId query string se lena — URL: /Student/Attendance?studentId=27
            // Agar nahi mila toh default 0 (koi record nahi dikhega)
            int studentId = 0;
            if (Request.Query.ContainsKey("studentId"))
                int.TryParse(Request.Query["studentId"], out studentId);

            if (studentId == 0)
            {
                // sessionStorage server side pe available nahi hota
                // Login system se student ka ID session/cookie mein store karo
                // Abhi ke liye pehla student le rahe hain as fallback
                var firstStudent = await _db.Students.FirstOrDefaultAsync();
                if (firstStudent != null)
                    studentId = firstStudent.Id;
            }

            var allRecords = await _db.Attendances
                .Where(a => a.StudentId == studentId)
                .OrderByDescending(a => a.Date)
                .ToListAsync();

            TotalPresent = allRecords.Count(a => a.Status == "Present");
            TotalAbsent = allRecords.Count(a => a.Status == "Absent");

            int total = TotalPresent + TotalAbsent;
            OverallRate = total > 0
                ? $"{(int)((double)TotalPresent / total * 100)}%"
                : "0%";

            RecentRecords = allRecords.Take(10).ToList();
        }
    }
}