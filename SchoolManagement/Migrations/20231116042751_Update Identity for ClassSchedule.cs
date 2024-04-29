using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagement.Migrations
{
    /// <inheritdoc />
    public partial class UpdateIdentityforClassSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassSchedules_Class_ClassScheduleId",
                table: "ClassSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassScheduleSlot_ClassSchedules_ClassScheduleId",
                table: "ClassScheduleSlot");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClassSchedules",
                table: "ClassSchedules");

            migrationBuilder.RenameTable(
                name: "ClassSchedules",
                newName: "ClassSchedule");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClassSchedule",
                table: "ClassSchedule",
                column: "ClassScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSchedule_Class_ClassScheduleId",
                table: "ClassSchedule",
                column: "ClassScheduleId",
                principalTable: "Class",
                principalColumn: "ClassId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassScheduleSlot_ClassSchedule_ClassScheduleId",
                table: "ClassScheduleSlot",
                column: "ClassScheduleId",
                principalTable: "ClassSchedule",
                principalColumn: "ClassScheduleId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassSchedule_Class_ClassScheduleId",
                table: "ClassSchedule");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassScheduleSlot_ClassSchedule_ClassScheduleId",
                table: "ClassScheduleSlot");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClassSchedule",
                table: "ClassSchedule");

            migrationBuilder.RenameTable(
                name: "ClassSchedule",
                newName: "ClassSchedules");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClassSchedules",
                table: "ClassSchedules",
                column: "ClassScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSchedules_Class_ClassScheduleId",
                table: "ClassSchedules",
                column: "ClassScheduleId",
                principalTable: "Class",
                principalColumn: "ClassId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassScheduleSlot_ClassSchedules_ClassScheduleId",
                table: "ClassScheduleSlot",
                column: "ClassScheduleId",
                principalTable: "ClassSchedules",
                principalColumn: "ClassScheduleId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
