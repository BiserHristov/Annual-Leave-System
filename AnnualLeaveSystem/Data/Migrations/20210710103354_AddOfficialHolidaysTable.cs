using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AnnualLeaveSystem.Data.Migrations
{
    public partial class AddOfficialHolidaysTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RequestComments",
                table: "Leaves",
                newName: "Comments");

            migrationBuilder.CreateTable(
                name: "OfficialHolidays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfficialHolidays", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OfficialHolidays");

            migrationBuilder.RenameColumn(
                name: "Comments",
                table: "Leaves",
                newName: "RequestComments");
        }
    }
}
