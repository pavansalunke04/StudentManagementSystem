using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;

// ✅ Alias add karo — conflict fix
using StudentEntity = StudentManagementSystem.Data.Student;

namespace StudentManagementSystem.Pages.Admin
{
    public class StudentsModel : PageModel
    {
        private readonly AppDbContext _db;
        public StudentsModel(AppDbContext db) { _db = db; }

        // ✅ StudentEntity use karo
        public List<StudentEntity> Students { get; set; } = new();

        public async Task OnGetAsync()
        {
            Students = await _db.Students.ToListAsync();
        }

        public async Task<IActionResult> OnPostAddAsync([FromBody] StudentRequest req)
        {
            if (req == null) return BadRequest();
            // ✅ StudentEntity use karo
            _db.Students.Add(new StudentEntity
            {
                Name = req.Name,
                Email = req.Email,
                Phone = req.Phone,
                RollNumber = req.RollNumber,
                Age = req.Age,
                Gender = req.Gender,
                CourseId = req.CourseId
            });
            await _db.SaveChangesAsync();
            return new JsonResult(new { success = true });
        }

        public async Task<IActionResult> OnPostEditAsync([FromBody] StudentRequest req)
        {
            var s = await _db.Students.FindAsync(req.Id);
            if (s == null) return NotFound();
            s.Name = req.Name;
            s.Email = req.Email;
            s.Phone = req.Phone;
            s.RollNumber = req.RollNumber;
            s.Age = req.Age;
            s.Gender = req.Gender;
            s.CourseId = req.CourseId;
            await _db.SaveChangesAsync();
            return new JsonResult(new { success = true });
        }

        public async Task<IActionResult> OnPostDeleteAsync([FromBody] DeleteRequest req)
        {
            var s = await _db.Students.FindAsync(req.Id);
            if (s == null) return NotFound();
            _db.Students.Remove(s);
            await _db.SaveChangesAsync();
            return new JsonResult(new { success = true });
        }
    }

    public class StudentRequest
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? RollNumber { get; set; }
        public int Age { get; set; }
        public string? Gender { get; set; }
        public int CourseId { get; set; }
    }

    public class DeleteRequest { public int Id { get; set; } }
}