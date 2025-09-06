using Microsoft.EntityFrameworkCore;
using University.Data.Data.Entities;

namespace University.Data.Data;

public class AppDbContext(
    DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Lecturer> Lecturers { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Faculty> Faculty { get; set; }
    public DbSet<UserCourse> UsersCourses { get; set; }
    public DbSet<UserLecturer> UsersLecturers { get; set; }
    public DbSet<CourseLecturer> CoursesLecturers { get; set; }
    public DbSet<FacultyCourse> FacultiesCourses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserCourse>()
            .HasKey(n => new { n.UserId, n.CourseId });
        modelBuilder.Entity<UserCourse>()
            .HasOne(u => u.User)
            .WithMany(k => k.UsersCourses)
            .HasForeignKey(i => i.UserId);
        modelBuilder.Entity<UserCourse>()
            .HasOne(c => c.Course)
            .WithMany(k => k.UsersCourses)
            .HasForeignKey(u => u.CourseId);

        modelBuilder.Entity<UserLecturer>()
            .HasKey(n => new { n.UserId, n.LecturerId });
        modelBuilder.Entity<UserLecturer>()
            .HasOne(u => u.User)
            .WithMany(k => k.UsersLecturers)
            .HasForeignKey(u => u.UserId);
        modelBuilder.Entity<UserLecturer>()
            .HasOne(l => l.Lecturer)
            .WithMany(k => k.UsersLecturers)
            .HasForeignKey(l => l.LecturerId);

        modelBuilder.Entity<CourseLecturer>()
            .HasKey(n => new { n.CourseId, n.LectureId });
        modelBuilder.Entity<CourseLecturer>()
            .HasOne(c => c.Course)
            .WithMany(k => k.CoursesLecturers)
            .HasForeignKey(c => c.CourseId);
        modelBuilder.Entity<CourseLecturer>()
            .HasOne(l => l.Lecturer)
            .WithMany(k => k.CoursesLecturers)
            .HasForeignKey(l => l.LectureId);

        modelBuilder.Entity<FacultyCourse>()
            .HasKey(n => new { n.CourseId, n.FacultyId });
        modelBuilder.Entity<FacultyCourse>()
            .HasOne(c => c.Course)
            .WithMany(k => k.FacultyCourses)
            .HasForeignKey(c => c.CourseId);
        modelBuilder.Entity<FacultyCourse>()
            .HasOne(l => l.Faculty)
            .WithMany(k => k.FacultyCourses)
            .HasForeignKey(l => l.FacultyId);


        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }
}