using Microsoft.EntityFrameworkCore.Migrations;

namespace AnnualLeaveSystem.Data.Migrations
{
    public partial class ChangeReplacementEmployeePropertyToSubstituteEmployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leaves_Employees_RequestEmployeeId",
                table: "Leaves");

            migrationBuilder.RenameColumn(
                name: "RequestEmployeeId",
                table: "Leaves",
                newName: "SubstituteEmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Leaves_RequestEmployeeId",
                table: "Leaves",
                newName: "IX_Leaves_SubstituteEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Leaves_Employees_SubstituteEmployeeId",
                table: "Leaves",
                column: "SubstituteEmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leaves_Employees_SubstituteEmployeeId",
                table: "Leaves");

            migrationBuilder.RenameColumn(
                name: "SubstituteEmployeeId",
                table: "Leaves",
                newName: "RequestEmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Leaves_SubstituteEmployeeId",
                table: "Leaves",
                newName: "IX_Leaves_RequestEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Leaves_Employees_RequestEmployeeId",
                table: "Leaves",
                column: "RequestEmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
