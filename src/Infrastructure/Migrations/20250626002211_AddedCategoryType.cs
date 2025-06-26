using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedCategoryType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "read_only",
                table: "categories");

            migrationBuilder.AddColumn<int>(
                name: "type",
                table: "categories",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "type",
                table: "categories");

            migrationBuilder.AddColumn<bool>(
                name: "read_only",
                table: "categories",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
