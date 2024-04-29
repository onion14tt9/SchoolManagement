using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagement.Migrations
{
    /// <inheritdoc />
    public partial class TransferGradefromUserCoursetoUserClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Grade",
                table: "UserCourse");

            migrationBuilder.AddColumn<double>(
                name: "Grade",
                table: "UserClass",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GradeModifiedBy",
                table: "UserClass",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "GradeModifiedDate",
                table: "UserClass",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Grade",
                table: "UserClass");

            migrationBuilder.DropColumn(
                name: "GradeModifiedBy",
                table: "UserClass");

            migrationBuilder.DropColumn(
                name: "GradeModifiedDate",
                table: "UserClass");

            migrationBuilder.AddColumn<double>(
                name: "Grade",
                table: "UserCourse",
                type: "float",
                nullable: true);
        }
    }
}
