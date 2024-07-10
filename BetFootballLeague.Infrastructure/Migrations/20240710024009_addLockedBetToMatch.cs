using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BetFootballLeague.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addLockedBetToMatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("ba3e373e-3a5c-4a99-8b82-c2263ca04bcd"));

            migrationBuilder.AddColumn<bool>(
                name: "IsLockedBet",
                table: "Matches",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Email", "FullName", "Password", "Phone", "Point", "Role", "Status", "UpdatedAt", "UpdatedBy", "Username" },
                values: new object[] { new Guid("dc74789a-d38e-440a-b593-b00447cc17ea"), new DateTime(2024, 7, 10, 9, 40, 8, 668, DateTimeKind.Local).AddTicks(2696), new Guid("00000000-0000-0000-0000-000000000000"), "nice231096@gmail.com", "Nguyen Thanh Long", "$2a$13$ZfdYFqhqhGev0XQveQZsQOOQF4gJSazxF1FTZnA.dguLGRFa.sPO6", "0348523140", 0, 0, 1, null, null, "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("dc74789a-d38e-440a-b593-b00447cc17ea"));

            migrationBuilder.DropColumn(
                name: "IsLockedBet",
                table: "Matches");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Email", "FullName", "Password", "Phone", "Point", "Role", "Status", "UpdatedAt", "UpdatedBy", "Username" },
                values: new object[] { new Guid("ba3e373e-3a5c-4a99-8b82-c2263ca04bcd"), new DateTime(2024, 7, 6, 1, 3, 20, 221, DateTimeKind.Local).AddTicks(4969), new Guid("00000000-0000-0000-0000-000000000000"), "nice231096@gmail.com", "Nguyen Thanh Long", "$2a$13$BIq.cLmE8nvaojxXxSAZGe52bbXrA764jy8j4B858DFB3s76TLbV6", "0348523140", 0, 0, 1, null, null, "admin" });
        }
    }
}
