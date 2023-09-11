using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RateACourse.Core.Migrations
{
    public partial class adminSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "00000000-0000-0000-0000-000000000001", "5bfcdb23-eaf5-43a4-b6fa-448ab068c061", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DateOfBirth", "Email", "EmailConfirmed", "Firstname", "Lastname", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "00000000-0000-0000-0000-000000000001", 0, "c8554266-b401-4519-9aeb-a9283053fc58", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@rateamovie.be", true, "Mike", "Admin", false, null, "ADMIN@RATEAMOVIE.BE", "ADMIN@RATEAMOVIE.BE", "AQAAAAEAACcQAAAAEPvTZ5TooBfIomWDDjCbKDP69rh98tIUvSHeRtQnjXGxJvri19XuqX/ifDRcLG+EZQ==", null, false, "VVPCRDAS3MJWQD5CSW2GWPRADBXEZINA", false, "admin@rateamovie.be" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "00000000-0000-0000-0000-000000000001", "00000000-0000-0000-0000-000000000001" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "00000000-0000-0000-0000-000000000001", "00000000-0000-0000-0000-000000000001" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000001");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000001");
        }
    }
}
