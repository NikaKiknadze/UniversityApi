using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace University.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserProfileEntity : Migration
    {
        private static readonly string[] userProfileColumns = new[] { "Id", "UserId", "FirstName", "LastName", "Age", "FacultyId" };
        private static readonly string[] userColumns = new[] { "Id", "PasswordHash", "Username", "IsActive" };

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                schema: "university",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "university",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "SurName",
                schema: "university",
                table: "Users",
                newName: "Username");

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                schema: "university",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                schema: "university",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    FacultyId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProfiles_Faculties_FacultyId",
                        column: x => x.FacultyId,
                        principalSchema: "university",
                        principalTable: "Faculties",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserProfiles_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "university",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_FacultyId",
                schema: "university",
                table: "UserProfiles",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_UserId",
                schema: "university",
                table: "UserProfiles",
                column: "UserId",
                unique: true);

            migrationBuilder.InsertData(
                table: "Users",
                schema: "university",
                columns: userColumns,
                values: new object[] { 1, "OSgVeEwwqvIOzITgNDP2MCIJnvKYyzB7dLEMjG64Cwg=", "admin", true });

            migrationBuilder.InsertData(
                table: "UserProfiles",
                schema: "university",
                columns: userProfileColumns,
                values: new object[] { 1, 1, "Admin", "Admin", 0, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserProfiles",
                schema: "university");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                schema: "university",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Username",
                schema: "university",
                table: "Users",
                newName: "SurName");

            migrationBuilder.AddColumn<int>(
                name: "Age",
                schema: "university",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "university",
                table: "Users",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }
    }
}
