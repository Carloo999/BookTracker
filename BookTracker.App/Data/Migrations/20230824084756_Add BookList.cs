using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookTracker.App.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBookList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookListId",
                schema: "Identity",
                table: "User",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "BookList",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Book",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Author = table.Column<string>(type: "TEXT", nullable: false),
                    Rating = table.Column<int>(type: "INTEGER", nullable: false),
                    Pages = table.Column<int>(type: "INTEGER", nullable: false),
                    PagesRead = table.Column<int>(type: "INTEGER", nullable: false),
                    BookListId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Book", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Book_BookList_BookListId",
                        column: x => x.BookListId,
                        principalSchema: "Identity",
                        principalTable: "BookList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_BookListId",
                schema: "Identity",
                table: "User",
                column: "BookListId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Book_BookListId",
                schema: "Identity",
                table: "Book",
                column: "BookListId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_BookList_BookListId",
                schema: "Identity",
                table: "User",
                column: "BookListId",
                principalSchema: "Identity",
                principalTable: "BookList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_BookList_BookListId",
                schema: "Identity",
                table: "User");

            migrationBuilder.DropTable(
                name: "Book",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "BookList",
                schema: "Identity");

            migrationBuilder.DropIndex(
                name: "IX_User_BookListId",
                schema: "Identity",
                table: "User");

            migrationBuilder.DropColumn(
                name: "BookListId",
                schema: "Identity",
                table: "User");
        }
    }
}
