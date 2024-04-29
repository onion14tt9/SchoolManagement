using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagement.Migrations
{
    /// <inheritdoc />
    public partial class RemoveClassCourseEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassCourse");

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "Class",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Class_CourseId",
                table: "Class",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Class_Course_CourseId",
                table: "Class",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Class_Course_CourseId",
                table: "Class");

            migrationBuilder.DropIndex(
                name: "IX_Class_CourseId",
                table: "Class");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Class");

            migrationBuilder.CreateTable(
                name: "ClassCourse",
                columns: table => new
                {
                    ClassId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassCourse", x => new { x.ClassId, x.CourseId });
                    table.ForeignKey(
                        name: "FK_ClassCourse_Class_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Class",
                        principalColumn: "ClassId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassCourse_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassCourse_CourseId",
                table: "ClassCourse",
                column: "CourseId");
        }
    }
}
