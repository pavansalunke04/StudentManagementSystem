using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;

namespace StudentManagementSystem.Pages.Admin
{
    public class MarksModel : PageModel
    {
        private readonly AppDbContext _db;
        public MarksModel(AppDbContext db) { _db = db; }

        public List<MarkViewModel> AllMarks { get; set; } = new();

        public async Task OnGetAsync()
        {
            AllMarks = await _db.Marks
                .Join(_db.Students,
                    m => m.StudentId,
                    s => s.Id,
                    (m, s) => new MarkViewModel
                    {
                        Id = m.Id,
                        StudentId = s.Id,
                        StudentName = s.Name ?? "",
                        RollNumber = s.RollNumber ?? "",
                        SubjectId = m.SubjectId,
                        ExamType = m.ExamType ?? "",
                        MarksObtained = m.MarksObtained,
                        TotalMarks = m.TotalMarks,
                        Grade = m.Grade ?? ""
                    })
                .ToListAsync();
        }

        // ✅ ADD
        public async Task<IActionResult> OnPostAddAsync([FromBody] MarksRequest req)
        {
            if (req == null) return BadRequest();
            _db.Marks.Add(new Marks
            {
                StudentId = req.StudentId,
                SubjectId = req.SubjectId,
                ExamType = req.ExamType,
                MarksObtained = req.MarksObtained,
                TotalMarks = req.TotalMarks,
                Grade = req.Grade
            });
            await _db.SaveChangesAsync();
            return new JsonResult(new { success = true });
        }

        // ✅ EDIT
        public async Task<IActionResult> OnPostEditAsync([FromBody] MarksRequest req)
        {
            var m = await _db.Marks.FindAsync(req.Id);
            if (m == null) return NotFound();
            m.MarksObtained = req.MarksObtained;
            m.TotalMarks = req.TotalMarks;
            m.Grade = req.Grade;
            m.ExamType = req.ExamType;
            await _db.SaveChangesAsync();
            return new JsonResult(new { success = true });
        }

        // ✅ DELETE
        public async Task<IActionResult> OnPostDeleteAsync([FromBody] DeleteRequest req)
        {
            var m = await _db.Marks.FindAsync(req.Id);
            if (m == null) return NotFound();
            _db.Marks.Remove(m);
            await _db.SaveChangesAsync();
            return new JsonResult(new { success = true });
        }
    }

    public class MarkViewModel
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; } = "";
        public string RollNumber { get; set; } = "";
        public int SubjectId { get; set; }
        public string ExamType { get; set; } = "";
        public int MarksObtained { get; set; }
        public int TotalMarks { get; set; }
        public string Grade { get; set; } = "";
    }

    public class MarksRequest
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public string? ExamType { get; set; }
        public int MarksObtained { get; set; }
        public int TotalMarks { get; set; }
        public string? Grade { get; set; }
    }
}