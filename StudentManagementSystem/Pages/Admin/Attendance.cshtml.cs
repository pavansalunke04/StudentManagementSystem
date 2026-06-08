using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;

namespace StudentManagementSystem.Pages.Admin
{
    public class AttendanceModel : PageModel
    {
        private readonly AppDbContext _db;

        public AttendanceModel(AppDbContext db)
        {
            _db = db;
        }

        public void OnGet() { }

        public class AttendanceRecord
        {
            public int StudentId { get; set; }
            public string? StudentName { get; set; }
            public string? Status { get; set; }
        }

        public class SaveAttendanceRequest
        {
            public string Date { get; set; } = "";
            public string? Course { get; set; }
            public List<AttendanceRecord> Records { get; set; } = new();
        }

        public async Task<IActionResult> OnPostSaveAttendanceAsync(
            [FromBody] SaveAttendanceRequest request)
        {
            if (!DateTime.TryParse(request.Date, out var date))
                return new JsonResult(new { success = false, message = "Invalid date" });

            var utcDate = DateTime.SpecifyKind(date, DateTimeKind.Utc);

            foreach (var rec in request.Records)
            {
                var existing = await _db.Attendances
                    .FirstOrDefaultAsync(a =>
                        a.StudentId == rec.StudentId &&
                        a.Date.Date == utcDate.Date &&
                        a.Course == request.Course);

                if (existing != null)
                {
                    existing.Status = rec.Status;
                    existing.StudentName = rec.StudentName;
                }
                else
                {
                    _db.Attendances.Add(new Attendance
                    {
                        StudentId = rec.StudentId,
                        StudentName = rec.StudentName,
                        Status = rec.Status,
                        Date = utcDate,
                        Course = request.Course,
                        SubjectId = 1
                    });
                }
            }

            await _db.SaveChangesAsync();
            return new JsonResult(new { success = true });
        }
    }
}