using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.Migrations
{
    public partial class users : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    authorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    authorName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    authorCountry = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.authorId);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    genreId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    genreName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.genreId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UsersId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsersFirstName = table.Column<string>(type: "varchar(13)", unicode: false, maxLength: 13, nullable: false),
                    UsersLastName = table.Column<string>(type: "varchar(13)", unicode: false, maxLength: 13, nullable: false),
                    UsersUserName = table.Column<string>(type: "varchar(11)", unicode: false, maxLength: 11, nullable: false),
                    UsersPassword = table.Column<string>(type: "varchar(16)", unicode: false, maxLength: 16, nullable: false),
                    UsersPhone = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    UsersEmail = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    UsersRole = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    UsersAddress = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    UserTzId = table.Column<string>(type: "varchar(9)", unicode: false, maxLength: 9, nullable: false),
                    UsersPasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    UsersPasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    UsersRefreshToken = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UsersTokenCreated = table.Column<DateTime>(type: "datetime", nullable: true),
                    UsersTokenExpires = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Borrowers", x => x.UsersId);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    bookId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    bookName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    authorId = table.Column<int>(type: "int", nullable: false),
                    bookPublishedYear = table.Column<int>(type: "int", nullable: false),
                    genreId = table.Column<int>(type: "int", nullable: false),
                    bookLanguage = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    bookNumOfPages = table.Column<int>(type: "int", nullable: false),
                    bookCopys = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.bookId);
                    table.ForeignKey(
                        name: "FK_Books_Authors",
                        column: x => x.authorId,
                        principalTable: "Authors",
                        principalColumn: "authorId");
                    table.ForeignKey(
                        name: "FK_Books_Genres",
                        column: x => x.genreId,
                        principalTable: "Genres",
                        principalColumn: "genreId");
                });

            migrationBuilder.CreateTable(
                name: "Borrowings",
                columns: table => new
                {
                    borrowingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    borrowingBookId = table.Column<int>(type: "int", nullable: false),
                    borrowingUserId = table.Column<int>(type: "int", nullable: false),
                    borrowingDate = table.Column<DateTime>(type: "date", nullable: false),
                    borrowingReturnDate = table.Column<DateTime>(type: "date", nullable: false),
                    borrowingReturnedDate = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Borrowings", x => x.borrowingId);
                    table.ForeignKey(
                        name: "FK_Borrowings_Books",
                        column: x => x.borrowingBookId,
                        principalTable: "Books",
                        principalColumn: "bookId");
                    table.ForeignKey(
                        name: "FK_Borrowings_Borrowers",
                        column: x => x.borrowingUserId,
                        principalTable: "Users",
                        principalColumn: "UsersId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_authorId",
                table: "Books",
                column: "authorId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_genreId",
                table: "Books",
                column: "genreId");

            migrationBuilder.CreateIndex(
                name: "IX_Borrowings_borrowingBookId",
                table: "Borrowings",
                column: "borrowingBookId");

            migrationBuilder.CreateIndex(
                name: "IX_Borrowings_borrowingUserId",
                table: "Borrowings",
                column: "borrowingUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Borrowings");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "Genres");
        }
    }
}
