using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELearning_Platform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changeAnswers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Test",
                schema: "Answers",
                table: "Test");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Test",
                schema: "Answers",
                table: "Test",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_Test_QuestionId",
                schema: "Answers",
                table: "Test",
                column: "QuestionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Test",
                schema: "Answers",
                table: "Test");

            migrationBuilder.DropIndex(
                name: "IX_Test_QuestionId",
                schema: "Answers",
                table: "Test");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Test",
                schema: "Answers",
                table: "Test",
                column: "QuestionId");
        }
    }
}
