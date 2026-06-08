using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;

// ✅ Alias add karo
using TeacherEntity = StudentManagementSystem.Data.Teacher;

namespace StudentManagementSystem.Pages.Admin
{
    public class TeachersModel : PageModel
    {
        private readonly AppDbContext _db;
        public TeachersModel(AppDbContext db) { _db = db; }

        // ✅ TeacherEntity use karo
        public List<TeacherEntity> Teachers { get; set; } = new();

        public async Task OnGetAsync()
        {
            Teachers = await _db.Teachers.ToListAsync();
        }

        public async Task<IActionResult> OnPostAddAsync([FromBody] TeacherRequest req)
        {
            if (req == null) return BadRequest();
            // ✅ TeacherEntity use karo
            _db.Teachers.Add(new TeacherEntity
            {
                Name = req.Name,
                Email = req.Email,
                Phone = req.Phone,
                Subject = req.Subject,
                Qualification = req.Qualification
            });
            await _db.SaveChangesAsync();
            return new JsonResult(new { success = true });
        }

        public async Task<IActionResult> OnPostEditAsync([FromBody] TeacherRequest req)
        {
            var t = await _db.Teachers.FindAsync(req.Id);
            if (t == null) return NotFound();
            t.Name = req.Name;
            t.Email = req.Email;
            t.Phone = req.Phone;
            t.Subject = req.Subject;
            t.Qualification = req.Qualification;
            await _db.SaveChangesAsync();
            return new JsonResult(new { success = true });
        }

        public async Task<IActionResult> OnPostDeleteAsync([FromBody] DeleteRequest req)
        {
            var t = await _db.Teachers.FindAsync(req.Id);
            if (t == null) return NotFound();
            _db.Teachers.Remove(t);
            await _db.SaveChangesAsync();
            return new JsonResult(new { success = true });
        }
    }

    public class TeacherRequest
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Subject { get; set; }
        public string? Qualification { get; set; }
    }
}