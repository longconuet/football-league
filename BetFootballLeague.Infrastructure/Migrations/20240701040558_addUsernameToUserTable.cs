using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BetFootballLeague.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addUsernameToUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("367e5cac-d5ce-4cba-892d-9a517567cda4"));

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Email", "FullName", "Phone", "Point", "Role", "Status", "UpdatedAt", "UpdatedBy", "Username" },
                values: new object[] { new Guid("b13c9938-761c-4cc0-b3ae-9bba860cb653"), new DateTime(2024, 7, 1, 11, 5, 58, 102, DateTimeKind.Local).AddTicks(6734), new Guid("00000000-0000-0000-0000-000000000000"), "nice231096@gmail.com", "Nguyen Thanh Long", "0348523140", 0, 0, 1, null, null, "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b13c9938-761c-4cc0-b3ae-9bba860cb653"));

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Users");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Email", "FullName", "Phone", "Point", "Role", "Status", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new Guid("367e5cac-d5ce-4cba-892d-9a517567cda4"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), "nice231096@gmail.com", "Nguyen Thanh Long", "0348523140", 0, 0, 1, null, null });
        }
    }
}
