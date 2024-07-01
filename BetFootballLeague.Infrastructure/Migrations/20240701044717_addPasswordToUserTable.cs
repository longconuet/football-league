using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BetFootballLeague.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addPasswordToUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b13c9938-761c-4cc0-b3ae-9bba860cb653"));

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Email", "FullName", "Password", "Phone", "Point", "Role", "Status", "UpdatedAt", "UpdatedBy", "Username" },
                values: new object[] { new Guid("dafd1b20-467d-4a7e-b5fe-2363630bc82e"), new DateTime(2024, 7, 1, 11, 47, 16, 299, DateTimeKind.Local).AddTicks(1780), new Guid("00000000-0000-0000-0000-000000000000"), "nice231096@gmail.com", "Nguyen Thanh Long", "$2a$13$NU07JmDn59tiROerrINhOeVHBFGpwxYfzFziyTYvO3Xb5lFMhvlxW", "0348523140", 0, 0, 1, null, null, "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("dafd1b20-467d-4a7e-b5fe-2363630bc82e"));

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Users");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Email", "FullName", "Phone", "Point", "Role", "Status", "UpdatedAt", "UpdatedBy", "Username" },
                values: new object[] { new Guid("b13c9938-761c-4cc0-b3ae-9bba860cb653"), new DateTime(2024, 7, 1, 11, 5, 58, 102, DateTimeKind.Local).AddTicks(6734), new Guid("00000000-0000-0000-0000-000000000000"), "nice231096@gmail.com", "Nguyen Thanh Long", "0348523140", 0, 0, 1, null, null, "admin" });
        }
    }
}
