using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace University.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLogTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "logs");

            migrationBuilder.EnsureSchema(
                name: "ums");

            migrationBuilder.RenameTable(
                name: "Users",
                schema: "university",
                newName: "Users",
                newSchema: "ums");

            migrationBuilder.RenameTable(
                name: "UserProfiles",
                schema: "university",
                newName: "UserProfiles",
                newSchema: "ums");

            migrationBuilder.RenameTable(
                name: "Lecturers",
                schema: "university",
                newName: "Lecturers",
                newSchema: "ums");

            migrationBuilder.CreateTable(
                name: "AuditEntries",
                schema: "logs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    EntityType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TableName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Action = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PrimaryKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditEntries_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "ums",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuditLog",
                schema: "logs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuditEntryId = table.Column<long>(type: "bigint", nullable: false),
                    TableName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Action = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ColumnName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OldValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditLog_AuditEntries_AuditEntryId",
                        column: x => x.AuditEntryId,
                        principalSchema: "logs",
                        principalTable: "AuditEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditEntries_UserId",
                schema: "logs",
                table: "AuditEntries",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_AuditEntryId",
                schema: "logs",
                table: "AuditLog",
                column: "AuditEntryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLog",
                schema: "logs");

            migrationBuilder.DropTable(
                name: "AuditEntries",
                schema: "logs");

            migrationBuilder.RenameTable(
                name: "Users",
                schema: "ums",
                newName: "Users",
                newSchema: "university");

            migrationBuilder.RenameTable(
                name: "UserProfiles",
                schema: "ums",
                newName: "UserProfiles",
                newSchema: "university");

            migrationBuilder.RenameTable(
                name: "Lecturers",
                schema: "ums",
                newName: "Lecturers",
                newSchema: "university");
        }
    }
}
