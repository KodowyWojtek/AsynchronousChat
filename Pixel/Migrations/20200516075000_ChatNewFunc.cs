using Microsoft.EntityFrameworkCore.Migrations;

namespace Pixel.Migrations
{
    public partial class ChatNewFunc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "UserFromRead",
                table: "MessageModel",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UserToRead",
                table: "MessageModel",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserFromRead",
                table: "MessageModel");

            migrationBuilder.DropColumn(
                name: "UserToRead",
                table: "MessageModel");
        }
    }
}
