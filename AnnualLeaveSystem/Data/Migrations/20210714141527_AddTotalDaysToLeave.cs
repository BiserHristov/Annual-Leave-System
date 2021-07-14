using Microsoft.EntityFrameworkCore.Migrations;

namespace AnnualLeaveSystem.Data.Migrations
{
    public partial class AddTotalDaysToLeave : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AppliedOn",
                table: "Leaves",
                newName: "RequestDate");

            migrationBuilder.AddColumn<int>(
                name: "TotalDays",
                table: "Leaves",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalDays",
                table: "Leaves");

            migrationBuilder.RenameColumn(
                name: "RequestDate",
                table: "Leaves",
                newName: "AppliedOn");
        }
    }
}
