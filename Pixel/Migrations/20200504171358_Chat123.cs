using Microsoft.EntityFrameworkCore.Migrations;

namespace Pixel.Migrations
{
    public partial class Chat123 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MessageModel",
                columns: table => new
                {
                    MessageId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserTo = table.Column<string>(type: "varchar(450)", nullable: false),
                    UserFrom = table.Column<string>(type: "varchar(450)", nullable: false),
                    Messsage = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageModel", x => x.MessageId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MessageModel");
        }
    }
}
