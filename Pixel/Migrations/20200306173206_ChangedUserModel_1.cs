using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pixel.Migrations
{
    public partial class ChangedUserModel_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "UsersModel");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "UsersModel",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfCreation",
                table: "UsersModel",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "UsersModel",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfCreation",
                table: "UsersModel");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "UsersModel");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "UsersModel",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "UsersModel",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
