using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELearning_Platform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addDbClasses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "School");

            migrationBuilder.EnsureSchema(
                name: "Lesson");

            migrationBuilder.AddColumn<Guid>(
                name: "ClassID",
                schema: "Person",
                table: "Person",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Class",
                schema: "School",
                columns: table => new
                {
                    ELearningClassID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    YearOfBeggining = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    YearOfEnded = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Class", x => x.ELearningClassID);
                });

            migrationBuilder.CreateTable(
                name: "Lessons",
                schema: "Lesson",
                columns: table => new
                {
                    LessonID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LessonTopic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LessonDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TeacherID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClassID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LessonDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lessons", x => x.LessonID);
                    table.ForeignKey(
                        name: "FK_Lessons_Class_ClassID",
                        column: x => x.ClassID,
                        principalSchema: "School",
                        principalTable: "Class",
                        principalColumn: "ELearningClassID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lessons_Person_TeacherID",
                        column: x => x.TeacherID,
                        principalSchema: "Person",
                        principalTable: "Person",
                        principalColumn: "AccountID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subject",
                schema: "School",
                columns: table => new
                {
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TeacherID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TeacherName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TeacherSurname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClassID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subject", x => x.SubjectId);
                    table.ForeignKey(
                        name: "FK_Subject_Class_ClassID",
                        column: x => x.ClassID,
                        principalSchema: "School",
                        principalTable: "Class",
                        principalColumn: "ELearningClassID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Subject_Person_TeacherID",
                        column: x => x.TeacherID,
                        principalSchema: "Person",
                        principalTable: "Person",
                        principalColumn: "AccountID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Materials",
                schema: "Lesson",
                columns: table => new
                {
                    LessonMaterialID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LessonID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.LessonMaterialID);
                    table.ForeignKey(
                        name: "FK_Materials_Lessons_LessonID",
                        column: x => x.LessonID,
                        principalSchema: "Lesson",
                        principalTable: "Lessons",
                        principalColumn: "LessonID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Person_ClassID",
                schema: "Person",
                table: "Person",
                column: "ClassID");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_ClassID",
                schema: "Lesson",
                table: "Lessons",
                column: "ClassID");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_TeacherID",
                schema: "Lesson",
                table: "Lessons",
                column: "TeacherID");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_LessonID",
                schema: "Lesson",
                table: "Materials",
                column: "LessonID");

            migrationBuilder.CreateIndex(
                name: "IX_Subject_ClassID",
                schema: "School",
                table: "Subject",
                column: "ClassID");

            migrationBuilder.CreateIndex(
                name: "IX_Subject_TeacherID",
                schema: "School",
                table: "Subject",
                column: "TeacherID");

            migrationBuilder.AddForeignKey(
                name: "FK_Person_Class_ClassID",
                schema: "Person",
                table: "Person",
                column: "ClassID",
                principalSchema: "School",
                principalTable: "Class",
                principalColumn: "ELearningClassID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Person_Class_ClassID",
                schema: "Person",
                table: "Person");

            migrationBuilder.DropTable(
                name: "Materials",
                schema: "Lesson");

            migrationBuilder.DropTable(
                name: "Subject",
                schema: "School");

            migrationBuilder.DropTable(
                name: "Lessons",
                schema: "Lesson");

            migrationBuilder.DropTable(
                name: "Class",
                schema: "School");

            migrationBuilder.DropIndex(
                name: "IX_Person_ClassID",
                schema: "Person",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "ClassID",
                schema: "Person",
                table: "Person");
        }
    }
}
