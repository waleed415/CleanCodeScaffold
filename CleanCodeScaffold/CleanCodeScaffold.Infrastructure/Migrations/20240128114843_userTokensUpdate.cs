using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanCodeScaffold.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class userTokensUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiredDateTimeUTC",
                table: "UserTokens",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "TokenTries",
                table: "UserTokens",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTokens_User_UserId",
                table: "UserTokens",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTokens_User_UserId",
                table: "UserTokens");

            migrationBuilder.DropColumn(
                name: "ExpiredDateTimeUTC",
                table: "UserTokens");

            migrationBuilder.DropColumn(
                name: "TokenTries",
                table: "UserTokens");
        }
    }
}
