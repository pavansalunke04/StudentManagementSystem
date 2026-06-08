using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseAndStudentName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Course",
                table: "attendances",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentName",
                table: "attendances",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Course",
                table: "attendances");

            migrationBuilder.DropColumn(
                name: "StudentName",
                table: "attendances");
        }
    }
}
