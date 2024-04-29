using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagement.Migrations
{
    /// <inheritdoc />
    public partial class TransfertoSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassScheduleSlot_ClassSchedule_ClassScheduleId",
                table: "ClassScheduleSlot");

            migrationBuilder.DropTable(
                name: "ClassSchedule");

            migrationBuilder.RenameColumn(
                name: "ClassScheduleId",
                table: "ClassScheduleSlot",
                newName: "ScheduleId");

            migrationBuilder.CreateTable(
                name: "Schedule",
                columns: table => new
                {
                    ScheduleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScheduleDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClassId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedule", x => x.ScheduleId);
                    table.ForeignKey(
                        name: "FK_Schedule_Class_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Class",
                        principalColumn: "ClassId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_ClassId",
                table: "Schedule",
                column: "ClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassScheduleSlot_Schedule_ScheduleId",
                table: "ClassScheduleSlot",
                column: "ScheduleId",
                principalTable: "Schedule",
                principalColumn: "ScheduleId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassScheduleSlot_Schedule_ScheduleId",
                table: "ClassScheduleSlot");

            migrationBuilder.DropTable(
                name: "Schedule");

            migrationBuilder.RenameColumn(
                name: "ScheduleId",
                table: "ClassScheduleSlot",
                newName: "ClassScheduleId");

            migrationBuilder.CreateTable(
                name: "ClassSchedule",
                columns: table => new
                {
                    ClassScheduleId = table.Column<int>(type: "int", nullable: false),
                    ClassId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ScheduleDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassSchedule", x => x.ClassScheduleId);
                    table.ForeignKey(
                        name: "FK_ClassSchedule_Class_ClassScheduleId",
                        column: x => x.ClassScheduleId,
                        principalTable: "Class",
                        principalColumn: "ClassId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ClassScheduleSlot_ClassSchedule_ClassScheduleId",
                table: "ClassScheduleSlot",
                column: "ClassScheduleId",
                principalTable: "ClassSchedule",
                principalColumn: "ClassScheduleId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
