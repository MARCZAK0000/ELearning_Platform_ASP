using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELearning_Platform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class teacherID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsComplited",
                schema: "Test",
                table: "Test",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TeacherID",
                schema: "Test",
                table: "Test",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Test_TeacherID",
                schema: "Test",
                table: "Test",
                column: "TeacherID");

            migrationBuilder.AddForeignKey(
                name: "FK_Test_Person_TeacherID",
                schema: "Test",
                table: "Test",
                column: "TeacherID",
                principalSchema: "Person",
                principalTable: "Person",
                principalColumn: "AccountID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Test_Person_TeacherID",
                schema: "Test",
                table: "Test");

            migrationBuilder.DropIndex(
                name: "IX_Test_TeacherID",
                schema: "Test",
                table: "Test");

            migrationBuilder.DropColumn(
                name: "IsComplited",
                schema: "Test",
                table: "Test");

            migrationBuilder.DropColumn(
                name: "TeacherID",
                schema: "Test",
                table: "Test");
        }
    }
}
