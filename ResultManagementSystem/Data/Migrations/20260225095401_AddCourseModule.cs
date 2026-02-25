using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResultManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_AcademicPrograms_AcademicProgramId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "SemesterNumber",
                table: "Courses");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_AcademicPrograms_AcademicProgramId",
                table: "Courses",
                column: "AcademicProgramId",
                principalTable: "AcademicPrograms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_AcademicPrograms_AcademicProgramId",
                table: "Courses");

            migrationBuilder.AddColumn<int>(
                name: "SemesterNumber",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_AcademicPrograms_AcademicProgramId",
                table: "Courses",
                column: "AcademicProgramId",
                principalTable: "AcademicPrograms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
