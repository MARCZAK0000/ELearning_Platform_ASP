using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELearning_Platform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changetestscore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorrectAnswerIndex",
                schema: "Questions",
                table: "Test");

            migrationBuilder.AddColumn<bool>(
                name: "IsCorrect",
                schema: "Answers",
                table: "Test",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCorrect",
                schema: "Answers",
                table: "Test");

            migrationBuilder.AddColumn<int>(
                name: "CorrectAnswerIndex",
                schema: "Questions",
                table: "Test",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
