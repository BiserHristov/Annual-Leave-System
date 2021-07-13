using Microsoft.EntityFrameworkCore.Migrations;

namespace AnnualLeaveSystem.Data.Migrations
{
    public partial class AddRequestedEmployeePropertyToLeaves : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RequestEmployeeId",
                table: "Leaves",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Leaves_RequestEmployeeId",
                table: "Leaves",
                column: "RequestEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Leaves_Employees_RequestEmployeeId",
                table: "Leaves",
                column: "RequestEmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leaves_Employees_RequestEmployeeId",
                table: "Leaves");

            migrationBuilder.DropIndex(
                name: "IX_Leaves_RequestEmployeeId",
                table: "Leaves");

            migrationBuilder.DropColumn(
                name: "RequestEmployeeId",
                table: "Leaves");
        }
    }
}
