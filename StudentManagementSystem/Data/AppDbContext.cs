using Microsoft.EntityFrameworkCore;

namespace StudentManagementSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Marks> Marks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ✅ PostgreSQL lowercase table name fix
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<Student>().ToTable("students");
            modelBuilder.Entity<Teacher>().ToTable("teachers");
            modelBuilder.Entity<Course>().ToTable("courses");
            modelBuilder.Entity<Subject>().ToTable("subjects");
            modelBuilder.Entity<Attendance>().ToTable("attendances");
            modelBuilder.Entity<Marks>().ToTable("marks");

            // ✅ User config
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Password).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Role).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.Username).IsUnique();
            });

            // ✅ Student config
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Email).HasMaxLength(150);
                entity.Property(e => e.Phone).HasMaxLength(20);
                entity.Property(e => e.RollNumber).HasMaxLength(50);
                entity.Property(e => e.Gender).HasMaxLength(20);
            });

            // ✅ Teacher config
            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Email).HasMaxLength(150);
                entity.Property(e => e.Phone).HasMaxLength(20);
                entity.Property(e => e.Subject).HasMaxLength(100);
                entity.Property(e => e.Qualification).HasMaxLength(150);
            });

            // ✅ Course config
            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Duration).HasMaxLength(50);
                entity.Property(e => e.Description).HasMaxLength(500);
            });

            // ✅ Subject config
            modelBuilder.Entity<Subject>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(150);
            });

            // ✅ Attendance config
            modelBuilder.Entity<Attendance>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Date).IsRequired();
            });

            // ✅ Marks config
            modelBuilder.Entity<Marks>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ExamType).HasMaxLength(50);
                entity.Property(e => e.Grade).HasMaxLength(5);
            });
        }
    }

    // ═══════════════════════════════════════
    //  ENTITY MODELS
    // ═══════════════════════════════════════

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }

    public class Student
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

    public class Teacher
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Subject { get; set; }
        public string? Qualification { get; set; }
    }

    public class Course
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Duration { get; set; }
        public string? Description { get; set; }
    }

    public class Subject
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int CourseId { get; set; }
        public int TeacherId { get; set; }
    }

    public class Attendance
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public string? StudentName { get; set; }
        public string? Course { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; } = "Present";
    }

    public class Marks
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