using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;          // ✅ ADD
using StudentManagementSystem.Data;

namespace StudentManagementSystem.Pages.Teacher
{
    public class AttendanceModel : PageModel
    {
        private readonly AppDbContext _context;

        public AttendanceModel(AppDbContext context)
        {
            _context = context;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostSaveAttendanceAsync(
            [FromBody] AttendanceRequest request)
        {
            if (request?.Records == null || request.Records.Count == 0)
                return BadRequest(new { success = false, message = "Koi data nahi mila!" });

            // ✅ UTC fix for PostgreSQL
            var requestDate = DateTime.SpecifyKind(request.Date, DateTimeKind.Utc);

            foreach (var record in request.Records)
            {
                // ✅ Async use karo - better performance
                var existing = await _context.Attendances
                    .FirstOrDefaultAsync(a =>
                        a.StudentId == record.StudentId &&
                        a.Date.Date == requestDate.Date);

                if (existing != null)
                {
                    existing.Status = record.Status;
                }
                else
                {
                    _context.Attendances.Add(new StudentManagementSystem.Data.Attendance
                    {
                        StudentId = record.StudentId,
                        SubjectId = 1,
                        Date = requestDate,
                        Status = record.Status
                    });
                }
            }

            await _context.SaveChangesAsync();

            return new JsonResult(new
            {
                success = true,
                message = "Attendance save ho gayi!"
            });
        }
    }

    public class AttendanceRequest
    {
        public DateTime Date { get; set; }
        public string? Course { get; set; }
        public List<AttendanceRecord> Records { get; set; } = new();
    }

    public class AttendanceRecord
    {
        public int StudentId { get; set; }
        public string? StudentName { get; set; }
        public string? Status { get; set; }
    }
}