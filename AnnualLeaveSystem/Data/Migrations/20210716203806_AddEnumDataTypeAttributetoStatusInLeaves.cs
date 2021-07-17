using Microsoft.EntityFrameworkCore.Migrations;

namespace AnnualLeaveSystem.Data.Migrations
{
    public partial class AddEnumDataTypeAttributetoStatusInLeaves : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Leaves",
                newName: "LeaveStatus");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LeaveStatus",
                table: "Leaves",
                newName: "Status");
        }
    }
}
