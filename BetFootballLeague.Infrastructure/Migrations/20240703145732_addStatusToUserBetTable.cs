using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BetFootballLeague.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addStatusToUserBetTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("dafd1b20-467d-4a7e-b5fe-2363630bc82e"));

            migrationBuilder.AddColumn<bool>(
                name: "IsWin",
                table: "UserBets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Email", "FullName", "Password", "Phone", "Point", "Role", "Status", "UpdatedAt", "UpdatedBy", "Username" },
                values: new object[] { new Guid("8500252b-6562-42fa-8baf-107f619dd05a"), new DateTime(2024, 7, 3, 21, 57, 31, 923, DateTimeKind.Local).AddTicks(4782), new Guid("00000000-0000-0000-0000-000000000000"), "nice231096@gmail.com", "Nguyen Thanh Long", "$2a$13$9m66.zY0Y.cPxt13osqIBOjQuSuB7i8O8xwt8g9SbhGu9KA6YAU0i", "0348523140", 0, 0, 1, null, null, "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_UserBets_BetTeamId",
                table: "UserBets",
                column: "BetTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBets_MatchId",
                table: "UserBets",
                column: "MatchId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBets_Matches_MatchId",
                table: "UserBets",
                column: "MatchId",
                principalTable: "Matches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBets_Teams_BetTeamId",
                table: "UserBets",
                column: "BetTeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBets_Matches_MatchId",
                table: "UserBets");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBets_Teams_BetTeamId",
                table: "UserBets");

            migrationBuilder.DropIndex(
                name: "IX_UserBets_BetTeamId",
                table: "UserBets");

            migrationBuilder.DropIndex(
                name: "IX_UserBets_MatchId",
                table: "UserBets");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("8500252b-6562-42fa-8baf-107f619dd05a"));

            migrationBuilder.DropColumn(
                name: "IsWin",
                table: "UserBets");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Email", "FullName", "Password", "Phone", "Point", "Role", "Status", "UpdatedAt", "UpdatedBy", "Username" },
                values: new object[] { new Guid("dafd1b20-467d-4a7e-b5fe-2363630bc82e"), new DateTime(2024, 7, 1, 11, 47, 16, 299, DateTimeKind.Local).AddTicks(1780), new Guid("00000000-0000-0000-0000-000000000000"), "nice231096@gmail.com", "Nguyen Thanh Long", "$2a$13$NU07JmDn59tiROerrINhOeVHBFGpwxYfzFziyTYvO3Xb5lFMhvlxW", "0348523140", 0, 0, 1, null, null, "admin" });
        }
    }
}
