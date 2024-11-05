using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELearning_Platform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changeOnDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Person_Id",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Grade_Person_AccountId",
                schema: "School",
                table: "Grade");

            migrationBuilder.DropForeignKey(
                name: "FK_Grade_Subject_SubjectID",
                schema: "School",
                table: "Grade");

            migrationBuilder.DropForeignKey(
                name: "FK_Grade_Test_TestID",
                schema: "School",
                table: "Grade");

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

            migrationBuilder.DropForeignKey(
                name: "FK_Test_Test_QuestionId",
                schema: "Answers",
                table: "Test");

            migrationBuilder.DropForeignKey(
                name: "FK_Test_Test_TestId",
                schema: "Questions",
                table: "Test");

            migrationBuilder.DropForeignKey(
                name: "FK_Test_Subject_SubjectID",
                schema: "Test",
                table: "Test");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Person_Id",
                table: "AspNetUsers",
                column: "Id",
                principalSchema: "Person",
                principalTable: "Person",
                principalColumn: "AccountID",
                onDelete: ReferentialAction.Restrict);

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
                name: "FK_Grade_Subject_SubjectID",
                schema: "School",
                table: "Grade",
                column: "SubjectID",
                principalSchema: "School",
                principalTable: "Subject",
                principalColumn: "SubjectId",
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

            migrationBuilder.AddForeignKey(
                name: "FK_Test_Subject_SubjectID",
                schema: "Test",
                table: "Test",
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
                name: "FK_AspNetUsers_Person_Id",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Grade_Person_AccountId",
                schema: "School",
                table: "Grade");

            migrationBuilder.DropForeignKey(
                name: "FK_Grade_Subject_SubjectID",
                schema: "School",
                table: "Grade");

            migrationBuilder.DropForeignKey(
                name: "FK_Grade_Test_TestID",
                schema: "School",
                table: "Grade");

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

            migrationBuilder.DropForeignKey(
                name: "FK_Test_Test_QuestionId",
                schema: "Answers",
                table: "Test");

            migrationBuilder.DropForeignKey(
                name: "FK_Test_Test_TestId",
                schema: "Questions",
                table: "Test");

            migrationBuilder.DropForeignKey(
                name: "FK_Test_Subject_SubjectID",
                schema: "Test",
                table: "Test");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Person_Id",
                table: "AspNetUsers",
                column: "Id",
                principalSchema: "Person",
                principalTable: "Person",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_Grade_Person_AccountId",
                schema: "School",
                table: "Grade",
                column: "AccountId",
                principalSchema: "Person",
                principalTable: "Person",
                principalColumn: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_Grade_Subject_SubjectID",
                schema: "School",
                table: "Grade",
                column: "SubjectID",
                principalSchema: "School",
                principalTable: "Subject",
                principalColumn: "SubjectId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Test_Test_QuestionId",
                schema: "Answers",
                table: "Test",
                column: "QuestionId",
                principalSchema: "Questions",
                principalTable: "Test",
                principalColumn: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Test_Test_TestId",
                schema: "Questions",
                table: "Test",
                column: "TestId",
                principalSchema: "Test",
                principalTable: "Test",
                principalColumn: "TestID");

            migrationBuilder.AddForeignKey(
                name: "FK_Test_Subject_SubjectID",
                schema: "Test",
                table: "Test",
                column: "SubjectID",
                principalSchema: "School",
                principalTable: "Subject",
                principalColumn: "SubjectId");
        }
    }
}
