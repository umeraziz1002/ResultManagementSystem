using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResultManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSemesterStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BatchId",
                table: "Semesters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Semesters_BatchId",
                table: "Semesters",
                column: "BatchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Semesters_Batches_BatchId",
                table: "Semesters",
                column: "BatchId",
                principalTable: "Batches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Semesters_Batches_BatchId",
                table: "Semesters");

            migrationBuilder.DropIndex(
                name: "IX_Semesters_BatchId",
                table: "Semesters");

            migrationBuilder.DropColumn(
                name: "BatchId",
                table: "Semesters");
        }
    }
}
