using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResultManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGradePolicyToExactMarks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxMarks",
                table: "GradePolicies");

            migrationBuilder.RenameColumn(
                name: "MinMarks",
                table: "GradePolicies",
                newName: "Marks");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Marks",
                table: "GradePolicies",
                newName: "MinMarks");

            migrationBuilder.AddColumn<decimal>(
                name: "MaxMarks",
                table: "GradePolicies",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }
    }
}
