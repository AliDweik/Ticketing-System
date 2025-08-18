using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketingSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddingUserUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserImagePath",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserImagePath",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
