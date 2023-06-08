using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CRM.Migrations
{
    public partial class f3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DOB",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "FeeAmount",
                table: "Students");

            migrationBuilder.AddColumn<string>(
                name: "StudentFee",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StudentFee",
                table: "Students");

            migrationBuilder.AddColumn<DateTime>(
                name: "DOB",
                table: "Students",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "FeeAmount",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
