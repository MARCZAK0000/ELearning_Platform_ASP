using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELearning_Platform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Teachers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "ModifiedDate",
                schema: "School",
                table: "Class",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.CreateTable(
                name: "ELearningClassUserInformations",
                columns: table => new
                {
                    ListOfTeachingClassesELearningClassID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeachersAccountID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ELearningClassUserInformations", x => new { x.ListOfTeachingClassesELearningClassID, x.TeachersAccountID });
                    table.ForeignKey(
                        name: "FK_ELearningClassUserInformations_Class_ListOfTeachingClassesELearningClassID",
                        column: x => x.ListOfTeachingClassesELearningClassID,
                        principalSchema: "School",
                        principalTable: "Class",
                        principalColumn: "ELearningClassID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ELearningClassUserInformations_Person_TeachersAccountID",
                        column: x => x.TeachersAccountID,
                        principalSchema: "Person",
                        principalTable: "Person",
                        principalColumn: "AccountID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ELearningClassUserInformations_TeachersAccountID",
                table: "ELearningClassUserInformations",
                column: "TeachersAccountID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ELearningClassUserInformations");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                schema: "School",
                table: "Class");
        }
    }
}
