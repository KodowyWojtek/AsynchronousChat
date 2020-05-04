using Microsoft.EntityFrameworkCore.Migrations;

namespace Pixel.Migrations
{
    public partial class Chat124 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Messsage",
                table: "MessageModel");

            migrationBuilder.AddColumn<string>(
                name: "MessageStore",
                table: "MessageModel",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MessageStore",
                table: "MessageModel");

            migrationBuilder.AddColumn<string>(
                name: "Messsage",
                table: "MessageModel",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
