using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BetFootballLeague.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateStatusUserBet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("8500252b-6562-42fa-8baf-107f619dd05a"));

            migrationBuilder.AlterColumn<bool>(
                name: "IsWin",
                table: "UserBets",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Email", "FullName", "Password", "Phone", "Point", "Role", "Status", "UpdatedAt", "UpdatedBy", "Username" },
                values: new object[] { new Guid("ba3e373e-3a5c-4a99-8b82-c2263ca04bcd"), new DateTime(2024, 7, 6, 1, 3, 20, 221, DateTimeKind.Local).AddTicks(4969), new Guid("00000000-0000-0000-0000-000000000000"), "nice231096@gmail.com", "Nguyen Thanh Long", "$2a$13$BIq.cLmE8nvaojxXxSAZGe52bbXrA764jy8j4B858DFB3s76TLbV6", "0348523140", 0, 0, 1, null, null, "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("ba3e373e-3a5c-4a99-8b82-c2263ca04bcd"));

            migrationBuilder.AlterColumn<bool>(
                name: "IsWin",
                table: "UserBets",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Email", "FullName", "Password", "Phone", "Point", "Role", "Status", "UpdatedAt", "UpdatedBy", "Username" },
                values: new object[] { new Guid("8500252b-6562-42fa-8baf-107f619dd05a"), new DateTime(2024, 7, 3, 21, 57, 31, 923, DateTimeKind.Local).AddTicks(4782), new Guid("00000000-0000-0000-0000-000000000000"), "nice231096@gmail.com", "Nguyen Thanh Long", "$2a$13$9m66.zY0Y.cPxt13osqIBOjQuSuB7i8O8xwt8g9SbhGu9KA6YAU0i", "0348523140", 0, 0, 1, null, null, "admin" });
        }
    }
}
