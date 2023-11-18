using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Abstractions;
using System.Data.Common;
using UniversityApi.Entities;

namespace UniversityApi.Data
{
    public class UniversistyContext : DbContext
    {
        public UniversistyContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Lecturer> Lecturers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Faculty> Faculty { get; set;}
        public DbSet<UsersCoursesJoin> UsersCoursesJoin { get;  set; }
        public DbSet<UsersLecturersJoin> UsersLecturersJoin { get; set; }
        public DbSet<CoursesLecturersJoin> CoursesLecturersJoin { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsersCoursesJoin>()
                .HasKey(n => new { n.UserId, n.CourseId});
            modelBuilder.Entity<UsersCoursesJoin>()
                .HasOne(u => u.User)
                .WithMany(k => k.UsersCourses)
                .HasForeignKey(i => i.UserId);
            modelBuilder.Entity<UsersCoursesJoin>()
                .HasOne(c => c.Course)
                .WithMany(k => k.UsersCourses)
                .HasForeignKey(u => u.CourseId);


            modelBuilder.Entity<UsersLecturersJoin>()
                .HasKey(n => new { n.UserId, n.LecturerId });
            modelBuilder.Entity<UsersLecturersJoin>()
                .HasOne(u => u.User)
                .WithMany(k => k.UsersLecturers)
                .HasForeignKey(u => u.UserId);
            modelBuilder.Entity<UsersLecturersJoin>()
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
            optionsBuilder.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
        }
    }


}
