using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELearning_Platform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NotificationInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Account");

            migrationBuilder.CreateTable(
                name: "Notification",
                schema: "Account",
                columns: table => new
                {
                    NotficaitonID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecipientID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SenderID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TimeSent = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsUnread = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.NotficaitonID);
                    table.ForeignKey(
                        name: "FK_Notification_Person_RecipientID",
                        column: x => x.RecipientID,
                        principalSchema: "Person",
                        principalTable: "Person",
                        principalColumn: "AccountID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notification_Person_SenderID",
                        column: x => x.SenderID,
                        principalSchema: "Person",
                        principalTable: "Person",
                        principalColumn: "AccountID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notification_RecipientID",
                schema: "Account",
                table: "Notification",
                column: "RecipientID");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_SenderID",
                schema: "Account",
                table: "Notification",
                column: "SenderID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notification",
                schema: "Account");
        }
    }
}
