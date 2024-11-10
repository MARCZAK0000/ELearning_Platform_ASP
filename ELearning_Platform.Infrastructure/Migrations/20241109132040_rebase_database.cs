using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELearning_Platform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class rebase_database : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grade_Person_AccountId",
                schema: "School",
                table: "Grade");

            migrationBuilder.DropForeignKey(
                name: "FK_Grade_Test_TestID",
                schema: "School",
                table: "Grade");

            migrationBuilder.DropForeignKey(
                name: "FK_Test_Test_QuestionId",
                schema: "Answers",
                table: "Test");

            migrationBuilder.DropForeignKey(
                name: "FK_Test_Test_TestId",
                schema: "Questions",
                table: "Test");

            migrationBuilder.DropIndex(
                name: "IX_Grade_GradeLevel",
                schema: "School",
                table: "Grade");

            migrationBuilder.DropColumn(
                name: "TeacherName",
                schema: "School",
                table: "Subject");

            migrationBuilder.DropColumn(
                name: "GradeLevel",
                schema: "School",
                table: "Grade");

            migrationBuilder.RenameColumn(
                name: "TeacherSurname",
                schema: "School",
                table: "Subject",
                newName: "StudentId");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                schema: "School",
                table: "Grade",
                newName: "StudentID");

            migrationBuilder.RenameIndex(
                name: "IX_Grade_AccountId",
                schema: "School",
                table: "Grade",
                newName: "IX_Grade_StudentID");

            migrationBuilder.AlterColumn<string>(
                name: "TeacherID",
                schema: "Test",
                table: "Test",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<Guid>(
                name: "SubjectID",
                schema: "Test",
                table: "Test",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "SubjectId",
                schema: "Person",
                table: "Person",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Person_SubjectId",
                schema: "Person",
                table: "Person",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Grade_Person_StudentID",
                schema: "School",
                table: "Grade",
                column: "StudentID",
                principalSchema: "Person",
                principalTable: "Person",
                principalColumn: "AccountID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Grade_Test_TestID",
                schema: "School",
                table: "Grade",
                column: "TestID",
                principalSchema: "Test",
                principalTable: "Test",
                principalColumn: "TestID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Person_Subject_SubjectId",
                schema: "Person",
                table: "Person",
                column: "SubjectId",
                principalSchema: "School",
                principalTable: "Subject",
                principalColumn: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Test_Test_QuestionId",
                schema: "Answers",
                table: "Test",
                column: "QuestionId",
                principalSchema: "Questions",
                principalTable: "Test",
                principalColumn: "QuestionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Test_Test_TestId",
                schema: "Questions",
                table: "Test",
                column: "TestId",
                principalSchema: "Test",
                principalTable: "Test",
                principalColumn: "TestID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grade_Person_StudentID",
                schema: "School",
                table: "Grade");

            migrationBuilder.DropForeignKey(
                name: "FK_Grade_Test_TestID",
                schema: "School",
                table: "Grade");

            migrationBuilder.DropForeignKey(
                name: "FK_Person_Subject_SubjectId",
                schema: "Person",
                table: "Person");

            migrationBuilder.DropForeignKey(
                name: "FK_Test_Test_QuestionId",
                schema: "Answers",
                table: "Test");

            migrationBuilder.DropForeignKey(
                name: "FK_Test_Test_TestId",
                schema: "Questions",
                table: "Test");

            migrationBuilder.DropIndex(
                name: "IX_Person_SubjectId",
                schema: "Person",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                schema: "Person",
                table: "Person");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                schema: "School",
                table: "Subject",
                newName: "TeacherSurname");

            migrationBuilder.RenameColumn(
                name: "StudentID",
                schema: "School",
                table: "Grade",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Grade_StudentID",
                schema: "School",
                table: "Grade",
                newName: "IX_Grade_AccountId");

            migrationBuilder.AlterColumn<string>(
                name: "TeacherID",
                schema: "Test",
                table: "Test",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "SubjectID",
                schema: "Test",
                table: "Test",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TeacherName",
                schema: "School",
                table: "Subject",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "GradeLevel",
                schema: "School",
                table: "Grade",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Grade_GradeLevel",
                schema: "School",
                table: "Grade",
                column: "GradeLevel");

            migrationBuilder.AddForeignKey(
                name: "FK_Grade_Person_AccountId",
                schema: "School",
                table: "Grade",
                column: "AccountId",
                principalSchema: "Person",
                principalTable: "Person",
                principalColumn: "AccountID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Grade_Test_TestID",
                schema: "School",
                table: "Grade",
                column: "TestID",
                principalSchema: "Test",
                principalTable: "Test",
                principalColumn: "TestID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Test_Test_QuestionId",
                schema: "Answers",
                table: "Test",
                column: "QuestionId",
                principalSchema: "Questions",
                principalTable: "Test",
                principalColumn: "QuestionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Test_Test_TestId",
                schema: "Questions",
                table: "Test",
                column: "TestId",
                principalSchema: "Test",
                principalTable: "Test",
                principalColumn: "TestID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
