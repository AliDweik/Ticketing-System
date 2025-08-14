using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketingSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AttachmentFiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "TicketAttachments",
                newName: "StoredFileName");

            migrationBuilder.AddColumn<string>(
                name: "OriginalFileName",
                table: "TicketAttachments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "Size",
                table: "TicketAttachments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OriginalFileName",
                table: "TicketAttachments");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "TicketAttachments");

            migrationBuilder.RenameColumn(
                name: "StoredFileName",
                table: "TicketAttachments",
                newName: "FileName");
        }
    }
}
