using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniversityApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Surname",
                schema: "university",
                table: "Lecturers",
                newName: "SurName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SurName",
                schema: "university",
                table: "Lecturers",
                newName: "Surname");
        }
    }
}
