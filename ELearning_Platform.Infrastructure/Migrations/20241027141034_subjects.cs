using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELearning_Platform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class subjects : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Surname",
                schema: "Person",
                table: "Person",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "EmailAddress",
                schema: "Person",
                table: "Person",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "SubjectID",
                schema: "Lesson",
                table: "Lessons",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "School",
                table: "Class",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Person_EmailAddress",
                schema: "Person",
                table: "Person",
                column: "EmailAddress",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Person_Surname",
                schema: "Person",
                table: "Person",
                column: "Surname");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_SubjectID",
                schema: "Lesson",
                table: "Lessons",
                column: "SubjectID");

            migrationBuilder.CreateIndex(
                name: "IX_Class_Name",
                schema: "School",
                table: "Class",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Subject_SubjectID",
                schema: "Lesson",
                table: "Lessons",
                column: "SubjectID",
                principalSchema: "School",
                principalTable: "Subject",
                principalColumn: "SubjectId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Subject_SubjectID",
                schema: "Lesson",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Person_EmailAddress",
                schema: "Person",
                table: "Person");

            migrationBuilder.DropIndex(
                name: "IX_Person_Surname",
                schema: "Person",
                table: "Person");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_SubjectID",
                schema: "Lesson",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Class_Name",
                schema: "School",
                table: "Class");

            migrationBuilder.DropColumn(
                name: "SubjectID",
                schema: "Lesson",
                table: "Lessons");

            migrationBuilder.AlterColumn<string>(
                name: "Surname",
                schema: "Person",
                table: "Person",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "EmailAddress",
                schema: "Person",
                table: "Person",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "School",
                table: "Class",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
