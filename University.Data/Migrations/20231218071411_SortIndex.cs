using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace University.Api.Migrations
{
    /// <inheritdoc />
    public partial class SortIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SortIndex",
                schema: "university",
                table: "Hierarchy",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SortIndex",
                schema: "university",
                table: "Hierarchy");
        }
    }
}
