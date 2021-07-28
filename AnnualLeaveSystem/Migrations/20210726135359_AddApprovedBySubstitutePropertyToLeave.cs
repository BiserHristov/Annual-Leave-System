using Microsoft.EntityFrameworkCore.Migrations;

namespace AnnualLeaveSystem.Migrations
{
    public partial class AddApprovedBySubstitutePropertyToLeave : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ApprovedBySubstitute",
                table: "Leaves",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovedBySubstitute",
                table: "Leaves");
        }
    }
}
