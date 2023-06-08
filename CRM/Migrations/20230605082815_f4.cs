using Microsoft.EntityFrameworkCore.Migrations;

namespace CRM.Migrations
{
    public partial class f4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discription",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discription",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Path",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Subject",
                table: "Students");
        }
    }
}
