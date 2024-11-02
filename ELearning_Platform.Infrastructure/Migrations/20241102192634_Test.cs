using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELearning_Platform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Person_Id",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Subject_SubjectID",
                schema: "Lesson",
                table: "Lessons");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Person_RecipientID",
                schema: "Account",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Person_SenderID",
                schema: "Account",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Person_Address_AccountID",
                schema: "Person",
                table: "Person");

            migrationBuilder.EnsureSchema(
                name: "Answers");

            migrationBuilder.EnsureSchema(
                name: "Questions");

            migrationBuilder.EnsureSchema(
                name: "Test");

            migrationBuilder.CreateTable(
                name: "Test",
                schema: "Test",
                columns: table => new
                {
                    TestID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubjectID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TestName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TestLevel = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Test", x => x.TestID);
                    table.ForeignKey(
                        name: "FK_Test_Subject_SubjectID",
                        column: x => x.SubjectID,
                        principalSchema: "School",
                        principalTable: "Subject",
                        principalColumn: "SubjectId");
                });

            migrationBuilder.CreateTable(
                name: "Grade",
                schema: "School",
                columns: table => new
                {
                    GradeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubjectID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TestID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GradeValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GradeLevel = table.Column<int>(type: "int", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grade", x => x.GradeID);
                    table.ForeignKey(
                        name: "FK_Grade_Person_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "Person",
                        principalTable: "Person",
                        principalColumn: "AccountID");
                    table.ForeignKey(
                        name: "FK_Grade_Subject_SubjectID",
                        column: x => x.SubjectID,
                        principalSchema: "School",
                        principalTable: "Subject",
                        principalColumn: "SubjectId");
                    table.ForeignKey(
                        name: "FK_Grade_Test_TestID",
                        column: x => x.TestID,
                        principalSchema: "Test",
                        principalTable: "Test",
                        principalColumn: "TestID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Test",
                schema: "Questions",
                columns: table => new
                {
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CorrectAnswerIndex = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Test", x => x.QuestionId);
                    table.ForeignKey(
                        name: "FK_Test_Test_TestId",
                        column: x => x.TestId,
                        principalSchema: "Test",
                        principalTable: "Test",
                        principalColumn: "TestID");
                });

            migrationBuilder.CreateTable(
                name: "Test",
                schema: "Answers",
                columns: table => new
                {
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnswerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnswerText = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Test", x => x.QuestionId);
                    table.ForeignKey(
                        name: "FK_Test_Test_QuestionId",
                        column: x => x.QuestionId,
                        principalSchema: "Questions",
                        principalTable: "Test",
                        principalColumn: "QuestionId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Grade_AccountId",
                schema: "School",
                table: "Grade",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Grade_GradeLevel",
                schema: "School",
                table: "Grade",
                column: "GradeLevel");

            migrationBuilder.CreateIndex(
                name: "IX_Grade_SubjectID",
                schema: "School",
                table: "Grade",
                column: "SubjectID");

            migrationBuilder.CreateIndex(
                name: "IX_Grade_TestID",
                schema: "School",
                table: "Grade",
                column: "TestID");

            migrationBuilder.CreateIndex(
                name: "IX_Test_TestId",
                schema: "Questions",
                table: "Test",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_Test_SubjectID",
                schema: "Test",
                table: "Test",
                column: "SubjectID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Person_Id",
                table: "AspNetUsers",
                column: "Id",
                principalSchema: "Person",
                principalTable: "Person",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Subject_SubjectID",
                schema: "Lesson",
                table: "Lessons",
                column: "SubjectID",
                principalSchema: "School",
                principalTable: "Subject",
                principalColumn: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Person_RecipientID",
                schema: "Account",
                table: "Notification",
                column: "RecipientID",
                principalSchema: "Person",
                principalTable: "Person",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Person_SenderID",
                schema: "Account",
                table: "Notification",
                column: "SenderID",
                principalSchema: "Person",
                principalTable: "Person",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_Person_Address_AccountID",
                schema: "Person",
                table: "Person",
                column: "AccountID",
                principalSchema: "Person",
                principalTable: "Address",
                principalColumn: "AccountID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Person_Id",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Subject_SubjectID",
                schema: "Lesson",
                table: "Lessons");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Person_RecipientID",
                schema: "Account",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Person_SenderID",
                schema: "Account",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Person_Address_AccountID",
                schema: "Person",
                table: "Person");

            migrationBuilder.DropTable(
                name: "Grade",
                schema: "School");

            migrationBuilder.DropTable(
                name: "Test",
                schema: "Answers");

            migrationBuilder.DropTable(
                name: "Test",
                schema: "Questions");

            migrationBuilder.DropTable(
                name: "Test",
                schema: "Test");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Person_Id",
                table: "AspNetUsers",
                column: "Id",
                principalSchema: "Person",
                principalTable: "Person",
                principalColumn: "AccountID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Subject_SubjectID",
                schema: "Lesson",
                table: "Lessons",
                column: "SubjectID",
                principalSchema: "School",
                principalTable: "Subject",
                principalColumn: "SubjectId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Person_RecipientID",
                schema: "Account",
                table: "Notification",
                column: "RecipientID",
                principalSchema: "Person",
                principalTable: "Person",
                principalColumn: "AccountID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Person_SenderID",
                schema: "Account",
                table: "Notification",
                column: "SenderID",
                principalSchema: "Person",
                principalTable: "Person",
                principalColumn: "AccountID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Person_Address_AccountID",
                schema: "Person",
                table: "Person",
                column: "AccountID",
                principalSchema: "Person",
                principalTable: "Address",
                principalColumn: "AccountID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
