using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookTracker.App.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangedUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                schema: "Identity",
                table: "User");

            migrationBuilder.DropColumn(
                name: "LastName",
                schema: "Identity",
                table: "User");

            migrationBuilder.AddColumn<bool>(
                name: "PrivacyStatus",
                schema: "Identity",
                table: "User",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrivacyStatus",
                schema: "Identity",
                table: "User");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                schema: "Identity",
                table: "User",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                schema: "Identity",
                table: "User",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
