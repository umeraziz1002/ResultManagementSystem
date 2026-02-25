using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResultManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMarkStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Marks_Enrollments_EnrollmentId",
                table: "Marks");

            migrationBuilder.AlterColumn<string>(
                name: "Grade",
                table: "Marks",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_Marks_Enrollments_EnrollmentId",
                table: "Marks",
                column: "EnrollmentId",
                principalTable: "Enrollments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Marks_Enrollments_EnrollmentId",
                table: "Marks");

            migrationBuilder.AlterColumn<string>(
                name: "Grade",
                table: "Marks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Marks_Enrollments_EnrollmentId",
                table: "Marks",
                column: "EnrollmentId",
                principalTable: "Enrollments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
