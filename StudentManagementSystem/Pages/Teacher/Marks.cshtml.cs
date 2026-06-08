using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;

// ✅ FIX: Pages/Student/ folder se namespace conflict — aliases use karo
using StudentEntity = StudentManagementSystem.Data.Student;
using SubjectEntity = StudentManagementSystem.Data.Subject;
using CourseEntity = StudentManagementSystem.Data.Course;
using MarksEntity = StudentManagementSystem.Data.Marks;

namespace StudentManagementSystem.Pages.Teacher
{
    public class MarksModel : PageModel
    {
        private readonly AppDbContext _db;
        public MarksModel(AppDbContext db) { _db = db; }

        // ✅ DB se data load karke View ko denge
        public List<StudentEntity> Students { get; set; } = new();
        public List<SubjectEntity> Subjects { get; set; } = new();
        public List<CourseEntity> Courses { get; set; } = new();

        public async Task OnGetAsync()
        {
            Students = await _db.Students.ToListAsync();
            Subjects = await _db.Subjects.ToListAsync();
            Courses = await _db.Courses.ToListAsync();
        }

        // ✅ POST: Marks save karta hai DB mein
        public async Task<IActionResult> OnPostSaveMarksAsync([FromBody] SaveMarksRequest req)
        {
            if (req == null || req.Marks == null || !req.Marks.Any())
                return BadRequest("No marks data received.");

            foreach (var m in req.Marks)
            {
                // Check karo: pehle se marks hain is student+subject ke liye?
                var existing = await _db.Marks
                    .FirstOrDefaultAsync(x => x.StudentId == m.StudentId && x.SubjectId == m.SubjectId);

                if (existing != null)
                {
                    // ✅ Update karo
                    existing.MarksObtained = m.MarksObtained;
                    existing.TotalMarks = 100;
                    existing.Grade = m.Grade;
                    existing.ExamType = m.ExamType ?? "Regular";
                }
                else
                {
                    // ✅ Naya record add karo
                    _db.Marks.Add(new MarksEntity
                    {
                        StudentId = m.StudentId,
                        SubjectId = m.SubjectId,
                        MarksObtained = m.MarksObtained,
                        TotalMarks = 100,
                        Grade = m.Grade,
                        ExamType = m.ExamType ?? "Regular"
                    });
                }
            }

            await _db.SaveChangesAsync();
            return new JsonResult(new { success = true });
        }
    }

    // ✅ Request body models
    public class SaveMarksRequest
    {
        public List<MarkEntry> Marks { get; set; } = new();
    }

    public class MarkEntry
    {
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public int MarksObtained { get; set; }
        public string? Grade { get; set; }
        public string? ExamType { get; set; }
    }
}
