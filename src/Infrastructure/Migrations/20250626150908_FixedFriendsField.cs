using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DecisionMate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixedFriendsField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_friends_profiles_user_profile1id",
                table: "friends");

            migrationBuilder.DropColumn(
                name: "friends",
                table: "profiles");

            migrationBuilder.RenameColumn(
                name: "user_profile1id",
                table: "friends",
                newName: "friends_id");

            migrationBuilder.AddForeignKey(
                name: "fk_friends_profiles_friends_id",
                table: "friends",
                column: "friends_id",
                principalTable: "profiles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_friends_profiles_friends_id",
                table: "friends");

            migrationBuilder.RenameColumn(
                name: "friends_id",
                table: "friends",
                newName: "user_profile1id");

            migrationBuilder.AddColumn<IReadOnlySet<Guid>>(
                name: "friends",
                table: "profiles",
                type: "uuid[]",
                nullable: false);

            migrationBuilder.AddForeignKey(
                name: "fk_friends_profiles_user_profile1id",
                table: "friends",
                column: "user_profile1id",
                principalTable: "profiles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
