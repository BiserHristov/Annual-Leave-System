using Microsoft.EntityFrameworkCore.Migrations;

namespace AnnualLeaveSystem.Migrations
{
    public partial class AddPendingApprovatDaysCountToEmployeeLeave : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PendingApprovalDays",
                table: "EmployeesLeaveTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PendingApprovalDays",
                table: "EmployeesLeaveTypes");
        }
    }
}
