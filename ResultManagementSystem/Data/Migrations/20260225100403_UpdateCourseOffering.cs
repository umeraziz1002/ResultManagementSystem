using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResultManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCourseOffering : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseOfferings_Courses_CourseId",
                table: "CourseOfferings");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseOfferings_Semesters_SemesterId",
                table: "CourseOfferings");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseOfferings_Courses_CourseId",
                table: "CourseOfferings",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseOfferings_Semesters_SemesterId",
                table: "CourseOfferings",
                column: "SemesterId",
                principalTable: "Semesters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseOfferings_Courses_CourseId",
                table: "CourseOfferings");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseOfferings_Semesters_SemesterId",
                table: "CourseOfferings");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseOfferings_Courses_CourseId",
                table: "CourseOfferings",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseOfferings_Semesters_SemesterId",
                table: "CourseOfferings",
                column: "SemesterId",
                principalTable: "Semesters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
