using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELearning_Platform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class userAnswers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "UserAnswers");

            migrationBuilder.CreateTable(
                name: "Test",
                schema: "UserAnswers",
                columns: table => new
                {
                    UserAnswerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TestID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnswerID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Test", x => x.UserAnswerID);
                    table.ForeignKey(
                        name: "FK_Test_Person_UserID",
                        column: x => x.UserID,
                        principalSchema: "Person",
                        principalTable: "Person",
                        principalColumn: "AccountID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Test_Test_AnswerID",
                        column: x => x.AnswerID,
                        principalSchema: "Answers",
                        principalTable: "Test",
                        principalColumn: "AnswerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Test_Test_QuestionID",
                        column: x => x.QuestionID,
                        principalSchema: "Questions",
                        principalTable: "Test",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Test_Test_TestID",
                        column: x => x.TestID,
                        principalSchema: "Test",
                        principalTable: "Test",
                        principalColumn: "TestID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Test_AnswerID",
                schema: "UserAnswers",
                table: "Test",
                column: "AnswerID",
                unique: true,
                filter: "[AnswerID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Test_QuestionID",
                schema: "UserAnswers",
                table: "Test",
                column: "QuestionID",
                unique: true,
                filter: "[QuestionID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Test_TestID",
                schema: "UserAnswers",
                table: "Test",
                column: "TestID");

            migrationBuilder.CreateIndex(
                name: "IX_Test_UserID",
                schema: "UserAnswers",
                table: "Test",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Test",
                schema: "UserAnswers");
        }
    }
}
