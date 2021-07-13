using Microsoft.EntityFrameworkCore.Migrations;

namespace AnnualLeaveSystem.Data.Migrations
{
    public partial class MakeApproveEmployeeNullableInLeave : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ApproveEmployeeId",
                table: "Leaves",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ApproveEmployeeId",
                table: "Leaves",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
