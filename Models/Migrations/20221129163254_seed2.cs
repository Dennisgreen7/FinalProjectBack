using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.Migrations
{
    public partial class seed2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UsersId",
                keyValue: 100);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UsersId", "UsersEmail", "UsersFirstName", "UsersLastName", "UsersPasswordHash", "UsersPasswordSalt", "UsersRefreshToken", "UsersRole", "UsersTokenCreated", "UsersTokenExpires", "UsersUserName" },
                values: new object[] { 1, "admin@gmail.com", "Admin", "Admin", new byte[] { 28, 235, 120, 90, 55, 21, 93, 197, 158, 25, 26, 92, 227, 253, 91, 186, 184, 158, 138, 129, 176, 145, 149, 62, 134, 212, 73, 103, 74, 227, 45, 19, 90, 230, 219, 155, 228, 173, 77, 0, 218, 170, 119, 30, 0, 44, 91, 67, 138, 63, 60, 169, 214, 188, 72, 178, 149, 17, 15, 223, 42, 91, 175, 57 }, new byte[] { 14, 152, 229, 126, 20, 212, 187, 57, 80, 108, 169, 21, 156, 129, 128, 16, 102, 41, 195, 226, 206, 255, 57, 241, 149, 182, 233, 69, 244, 10, 101, 254, 34, 21, 108, 88, 133, 184, 68, 46, 45, 235, 223, 51, 246, 179, 114, 205, 56, 108, 37, 51, 189, 112, 58, 81, 90, 236, 197, 24, 146, 59, 127, 134, 219, 3, 64, 204, 149, 182, 70, 249, 157, 32, 9, 28, 140, 164, 155, 65, 192, 130, 70, 196, 75, 3, 185, 169, 70, 81, 218, 9, 225, 142, 250, 213, 35, 59, 207, 94, 241, 50, 188, 88, 193, 65, 143, 18, 166, 133, 16, 10, 207, 202, 105, 203, 37, 79, 54, 178, 194, 73, 185, 11, 37, 97, 189, 14 }, null, "Admin", null, null, "Admin23456" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UsersId",
                keyValue: 1);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UsersId", "UsersEmail", "UsersFirstName", "UsersLastName", "UsersPasswordHash", "UsersPasswordSalt", "UsersRefreshToken", "UsersRole", "UsersTokenCreated", "UsersTokenExpires", "UsersUserName" },
                values: new object[] { 100, "admin@gmail.com", "Admin", "Admin", null, null, null, "Admin", null, null, "Admin23456" });
        }
    }
}
