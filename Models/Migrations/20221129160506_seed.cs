using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.Migrations
{
    public partial class seed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UsersId", "UsersEmail", "UsersFirstName", "UsersLastName", "UsersPasswordHash", "UsersPasswordSalt", "UsersRefreshToken", "UsersRole", "UsersTokenCreated", "UsersTokenExpires", "UsersUserName" },
                values: new object[] { 100, "admin@gmail.com", "Admin", "Admin", null, null, null, "Admin", null, null, "Admin23456" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UsersId",
                keyValue: 100);
        }
    }
}
