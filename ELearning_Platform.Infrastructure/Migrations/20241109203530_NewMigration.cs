using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELearning_Platform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NewMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Person_Subject_SubjectId",
                schema: "Person",
                table: "Person");

            migrationBuilder.DropIndex(
                name: "IX_Person_SubjectId",
                schema: "Person",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "StudentId",
                schema: "School",
                table: "Subject");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                schema: "Person",
                table: "Person");

            migrationBuilder.CreateTable(
                name: "StudentSubject",
                schema: "School",
                columns: table => new
                {
                    StudentSubjectId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SubjectID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentSubject", x => x.StudentSubjectId);
                    table.ForeignKey(
                        name: "FK_StudentSubject_Person_StudentID",
                        column: x => x.StudentID,
                        principalSchema: "Person",
                        principalTable: "Person",
                        principalColumn: "AccountID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentSubject_Subject_SubjectID",
                        column: x => x.SubjectID,
                        principalSchema: "School",
                        principalTable: "Subject",
                        principalColumn: "SubjectId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentSubject_StudentID",
                schema: "School",
                table: "StudentSubject",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_StudentSubject_SubjectID",
                schema: "School",
                table: "StudentSubject",
                column: "SubjectID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentSubject",
                schema: "School");

            migrationBuilder.AddColumn<string>(
                name: "StudentId",
                schema: "School",
                table: "Subject",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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
                name: "FK_Person_Subject_SubjectId",
                schema: "Person",
                table: "Person",
                column: "SubjectId",
                principalSchema: "School",
                principalTable: "Subject",
                principalColumn: "SubjectId");
        }
    }
}
