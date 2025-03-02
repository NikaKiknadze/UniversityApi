using Microsoft.EntityFrameworkCore;
using University.Data.Data.Entities;

namespace University.Data
{
    public class UniversityDbContext(DbContextOptions<UniversityDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Lecturer> Lecturers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Faculty> Faculty { get; set;}
        public DbSet<UsersCourses> UsersCoursesJoin { get;  set; }
        public DbSet<UsersLecturers> UsersLecturersJoin { get; set; }
        public DbSet<CoursesLecturersJoin> CoursesLecturersJoin { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsersCourses>()
                .HasKey(n => new { n.UserId, n.CourseId});
            modelBuilder.Entity<UsersCourses>()
                .HasOne(u => u.User)
                .WithMany(k => k.UsersCourses)
                .HasForeignKey(i => i.UserId);
            modelBuilder.Entity<UsersCourses>()
                .HasOne(c => c.Course)
                .WithMany(k => k.UsersCourses)
                .HasForeignKey(u => u.CourseId);


            modelBuilder.Entity<UsersLecturers>()
                .HasKey(n => new { n.UserId, n.LecturerId });
            modelBuilder.Entity<UsersLecturers>()
                .HasOne(u => u.User)
                .WithMany(k => k.UsersLecturers)
                .HasForeignKey(u => u.UserId);
            modelBuilder.Entity<UsersLecturers>()
                .HasOne(l => l.Lecturer)
                .WithMany(k => k.UsersLecturers)
                .HasForeignKey(l => l.LecturerId);


            modelBuilder.Entity<CoursesLecturersJoin>()
                .HasKey(n => new { n.CourseId, n.LectureId });
            modelBuilder.Entity<CoursesLecturersJoin>()
                .HasOne(c => c.Course)
                .WithMany(k => k.CoursesLecturers)
                .HasForeignKey(c => c.CourseId);
            modelBuilder.Entity<CoursesLecturersJoin>()
                .HasOne(l => l.Lecturer)
                .WithMany(k => k.CoursesLecturers)
                .HasForeignKey(l => l.LectureId);


            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // optionsBuilder.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
        }
    }


}
