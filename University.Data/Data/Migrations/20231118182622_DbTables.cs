using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace University.Api.Migrations
{
    /// <inheritdoc />
    public partial class DbTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "university");

            migrationBuilder.CreateTable(
                name: "Faculties",
                schema: "university",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FacultyName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faculties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lecturers",
                schema: "university",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lecturers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                schema: "university",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FacultyId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courses_Faculties_FacultyId",
                        column: x => x.FacultyId,
                        principalSchema: "university",
                        principalTable: "Faculties",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "university",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SurName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    FacultyId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Faculties_FacultyId",
                        column: x => x.FacultyId,
                        principalSchema: "university",
                        principalTable: "Faculties",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CoursesLecturersJoin",
                schema: "university",
                columns: table => new
                {
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    LectureId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoursesLecturersJoin", x => new { x.CourseId, x.LectureId });
                    table.ForeignKey(
                        name: "FK_CoursesLecturersJoin_Courses_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "university",
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CoursesLecturersJoin_Lecturers_LectureId",
                        column: x => x.LectureId,
                        principalSchema: "university",
                        principalTable: "Lecturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsersCoursesJoin",
                schema: "university",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersCoursesJoin", x => new { x.UserId, x.CourseId });
                    table.ForeignKey(
                        name: "FK_UsersCoursesJoin_Courses_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "university",
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersCoursesJoin_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "university",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsersLecturersJoin",
                schema: "university",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LecturerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersLecturersJoin", x => new { x.UserId, x.LecturerId });
                    table.ForeignKey(
                        name: "FK_UsersLecturersJoin_Lecturers_LecturerId",
                        column: x => x.LecturerId,
                        principalSchema: "university",
                        principalTable: "Lecturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersLecturersJoin_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "university",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_FacultyId",
                schema: "university",
                table: "Courses",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_CoursesLecturersJoin_LectureId",
                schema: "university",
                table: "CoursesLecturersJoin",
                column: "LectureId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_FacultyId",
                schema: "university",
                table: "Users",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersCoursesJoin_CourseId",
                schema: "university",
                table: "UsersCoursesJoin",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersLecturersJoin_LecturerId",
                schema: "university",
                table: "UsersLecturersJoin",
                column: "LecturerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoursesLecturersJoin",
                schema: "university");

            migrationBuilder.DropTable(
                name: "UsersCoursesJoin",
                schema: "university");

            migrationBuilder.DropTable(
                name: "UsersLecturersJoin",
                schema: "university");

            migrationBuilder.DropTable(
                name: "Courses",
                schema: "university");

            migrationBuilder.DropTable(
                name: "Lecturers",
                schema: "university");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "university");

            migrationBuilder.DropTable(
                name: "Faculties",
                schema: "university");
        }
    }
}
