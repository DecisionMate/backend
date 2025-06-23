using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddReadonlyCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<IReadOnlySet<Guid>>(
                name: "friends",
                table: "profiles",
                type: "uuid[]",
                nullable: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "profiles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "decision_games",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "categories",
                type: "character varying(1024)",
                maxLength: 1024,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(1024)",
                oldMaxLength: 1024);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "categories",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "read_only",
                table: "categories",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "friends",
                table: "profiles");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "profiles");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "decision_games");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "categories");

            migrationBuilder.DropColumn(
                name: "read_only",
                table: "categories");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "categories",
                type: "character varying(1024)",
                maxLength: 1024,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(1024)",
                oldMaxLength: 1024,
                oldNullable: true);
        }
    }
}
