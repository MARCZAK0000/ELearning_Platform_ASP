using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELearning_Platform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeBehaviors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Person_Id",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Person_Address_AccountID",
                schema: "Person",
                table: "Person");

            migrationBuilder.AlterColumn<string>(
                name: "SenderID",
                schema: "Account",
                table: "Notification",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "RecipientID",
                schema: "Account",
                table: "Notification",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Person_Id",
                table: "AspNetUsers",
                column: "Id",
                principalSchema: "Person",
                principalTable: "Person",
                principalColumn: "AccountID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Person_Address_AccountID",
                schema: "Person",
                table: "Person",
                column: "AccountID",
                principalSchema: "Person",
                principalTable: "Address",
                principalColumn: "AccountID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Person_Id",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Person_Address_AccountID",
                schema: "Person",
                table: "Person");

            migrationBuilder.AlterColumn<string>(
                name: "SenderID",
                schema: "Account",
                table: "Notification",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RecipientID",
                schema: "Account",
                table: "Notification",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Person_Id",
                table: "AspNetUsers",
                column: "Id",
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
