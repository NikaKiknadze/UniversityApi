﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UniversityApi.Data;

#nullable disable

namespace UniversityApi.Migrations
{
    [DbContext(typeof(UniversistyContext))]
    [Migration("20231218071411_SortIndex")]
    partial class SortIndex
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("UniversityApi.Entities.Course", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CourseName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("FacultyId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FacultyId");

                    b.ToTable("Courses", "university");
                });

            modelBuilder.Entity("UniversityApi.Entities.CoursesLecturersJoin", b =>
                {
                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<int>("LectureId")
                        .HasColumnType("int");

                    b.HasKey("CourseId", "LectureId");

                    b.HasIndex("LectureId");

                    b.ToTable("CoursesLecturersJoin", "university");
                });

            modelBuilder.Entity("UniversityApi.Entities.Faculty", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("FacultyName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Faculties", "university");
                });

            modelBuilder.Entity("UniversityApi.Entities.Hierarchy", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("ParentId")
                        .HasColumnType("int");

                    b.Property<int>("SortIndex")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Hierarchy", "university");
                });

            modelBuilder.Entity("UniversityApi.Entities.Lecturer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("SurName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.ToTable("Lecturers", "university");
                });

            modelBuilder.Entity("UniversityApi.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<int?>("FacultyId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("SurName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("FacultyId");

                    b.ToTable("Users", "university");
                });

            modelBuilder.Entity("UniversityApi.Entities.UsersCoursesJoin", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "CourseId");

                    b.HasIndex("CourseId");

                    b.ToTable("UsersCoursesJoin", "university");
                });

            modelBuilder.Entity("UniversityApi.Entities.UsersLecturersJoin", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("LecturerId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "LecturerId");

                    b.HasIndex("LecturerId");

                    b.ToTable("UsersLecturersJoin", "university");
                });

            modelBuilder.Entity("UniversityApi.Entities.Course", b =>
                {
                    b.HasOne("UniversityApi.Entities.Faculty", "Faculty")
                        .WithMany("Courses")
                        .HasForeignKey("FacultyId");

                    b.Navigation("Faculty");
                });

            modelBuilder.Entity("UniversityApi.Entities.CoursesLecturersJoin", b =>
                {
                    b.HasOne("UniversityApi.Entities.Course", "Course")
                        .WithMany("CoursesLecturers")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UniversityApi.Entities.Lecturer", "Lecturer")
                        .WithMany("CoursesLecturers")
                        .HasForeignKey("LectureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Lecturer");
                });

            modelBuilder.Entity("UniversityApi.Entities.User", b =>
                {
                    b.HasOne("UniversityApi.Entities.Faculty", "Faculty")
                        .WithMany("Users")
                        .HasForeignKey("FacultyId");

                    b.Navigation("Faculty");
                });

            modelBuilder.Entity("UniversityApi.Entities.UsersCoursesJoin", b =>
                {
                    b.HasOne("UniversityApi.Entities.Course", "Course")
                        .WithMany("UsersCourses")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UniversityApi.Entities.User", "User")
                        .WithMany("UsersCourses")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("User");
                });

            modelBuilder.Entity("UniversityApi.Entities.UsersLecturersJoin", b =>
                {
                    b.HasOne("UniversityApi.Entities.Lecturer", "Lecturer")
                        .WithMany("UsersLecturers")
                        .HasForeignKey("LecturerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UniversityApi.Entities.User", "User")
                        .WithMany("UsersLecturers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lecturer");

                    b.Navigation("User");
                });

            modelBuilder.Entity("UniversityApi.Entities.Course", b =>
                {
                    b.Navigation("CoursesLecturers");

                    b.Navigation("UsersCourses");
                });

            modelBuilder.Entity("UniversityApi.Entities.Faculty", b =>
                {
                    b.Navigation("Courses");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("UniversityApi.Entities.Lecturer", b =>
                {
                    b.Navigation("CoursesLecturers");

                    b.Navigation("UsersLecturers");
                });

            modelBuilder.Entity("UniversityApi.Entities.User", b =>
                {
                    b.Navigation("UsersCourses");

                    b.Navigation("UsersLecturers");
                });
#pragma warning restore 612, 618
        }
    }
}
