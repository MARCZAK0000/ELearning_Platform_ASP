using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ELearning_Platform.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class INIT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Person");

            migrationBuilder.EnsureSchema(
                name: "School");

            migrationBuilder.EnsureSchema(
                name: "Lesson");

            migrationBuilder.EnsureSchema(
                name: "Account");

            migrationBuilder.EnsureSchema(
                name: "GradeDetails");

            migrationBuilder.EnsureSchema(
                name: "Answers");

            migrationBuilder.EnsureSchema(
                name: "Questions");

            migrationBuilder.EnsureSchema(
                name: "Test");

            migrationBuilder.EnsureSchema(
                name: "UserAnswers");

            migrationBuilder.CreateTable(
                name: "Address",
                schema: "Person",
                columns: table => new
                {
                    AccountID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StreetName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.AccountID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Class",
                schema: "School",
                columns: table => new
                {
                    ELearningClassID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    YearOfBeggining = table.Column<int>(type: "int", nullable: false),
                    YearOfEnding = table.Column<int>(type: "int", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Class", x => x.ELearningClassID);
                });

            migrationBuilder.CreateTable(
                name: "School",
                schema: "GradeDetails",
                columns: table => new
                {
                    GradeID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GradePointsScore = table.Column<int>(type: "int", nullable: false),
                    TestQuestionsCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_School", x => x.GradeID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Person",
                schema: "Person",
                columns: table => new
                {
                    AccountID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SecondName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClassID = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.AccountID);
                    table.ForeignKey(
                        name: "FK_Person_Address_AccountID",
                        column: x => x.AccountID,
                        principalSchema: "Person",
                        principalTable: "Address",
                        principalColumn: "AccountID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Person_Class_ClassID",
                        column: x => x.ClassID,
                        principalSchema: "School",
                        principalTable: "Class",
                        principalColumn: "ELearningClassID");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Person_Id",
                        column: x => x.Id,
                        principalSchema: "Person",
                        principalTable: "Person",
                        principalColumn: "AccountID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ELearningClassUserInformations",
                columns: table => new
                {
                    ListOfTeachingClassesELearningClassID = table.Column<string>(type: "nvarchar(450)", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "Notification",
                schema: "Account",
                columns: table => new
                {
                    NotficaitonID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecipientID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SenderID = table.Column<string>(type: "nvarchar(450)", nullable: true),
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

            migrationBuilder.CreateTable(
                name: "Subject",
                schema: "School",
                columns: table => new
                {
                    SubjectId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClassID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TeacherID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subject", x => x.SubjectId);
                    table.ForeignKey(
                        name: "FK_Subject_Class_ClassID",
                        column: x => x.ClassID,
                        principalSchema: "School",
                        principalTable: "Class",
                        principalColumn: "ELearningClassID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Subject_Person_TeacherID",
                        column: x => x.TeacherID,
                        principalSchema: "Person",
                        principalTable: "Person",
                        principalColumn: "AccountID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lessons",
                schema: "Lesson",
                columns: table => new
                {
                    LessonID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LessonTopic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LessonDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TeacherID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClassID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SubjectID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LessonDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lessons", x => x.LessonID);
                    table.ForeignKey(
                        name: "FK_Lessons_Class_ClassID",
                        column: x => x.ClassID,
                        principalSchema: "School",
                        principalTable: "Class",
                        principalColumn: "ELearningClassID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lessons_Person_TeacherID",
                        column: x => x.TeacherID,
                        principalSchema: "Person",
                        principalTable: "Person",
                        principalColumn: "AccountID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lessons_Subject_SubjectID",
                        column: x => x.SubjectID,
                        principalSchema: "School",
                        principalTable: "Subject",
                        principalColumn: "SubjectId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudentSubject",
                schema: "School",
                columns: table => new
                {
                    StudentSubjectId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SubjectID = table.Column<string>(type: "nvarchar(450)", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Test",
                schema: "Test",
                columns: table => new
                {
                    TestID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SubjectID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TestName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TestLevel = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsComplited = table.Column<bool>(type: "bit", nullable: false),
                    TeacherID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Test", x => x.TestID);
                    table.ForeignKey(
                        name: "FK_Test_Person_TeacherID",
                        column: x => x.TeacherID,
                        principalSchema: "Person",
                        principalTable: "Person",
                        principalColumn: "AccountID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Test_Subject_SubjectID",
                        column: x => x.SubjectID,
                        principalSchema: "School",
                        principalTable: "Subject",
                        principalColumn: "SubjectId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Materials",
                schema: "Lesson",
                columns: table => new
                {
                    LessonMaterialID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LessonID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.LessonMaterialID);
                    table.ForeignKey(
                        name: "FK_Materials_Lessons_LessonID",
                        column: x => x.LessonID,
                        principalSchema: "Lesson",
                        principalTable: "Lessons",
                        principalColumn: "LessonID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Grade",
                schema: "School",
                columns: table => new
                {
                    GradeID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SubjectID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StudentID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TestID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GradeValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grade", x => x.GradeID);
                    table.ForeignKey(
                        name: "FK_Grade_Person_StudentID",
                        column: x => x.StudentID,
                        principalSchema: "Person",
                        principalTable: "Person",
                        principalColumn: "AccountID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Grade_School_GradeID",
                        column: x => x.GradeID,
                        principalSchema: "GradeDetails",
                        principalTable: "School",
                        principalColumn: "GradeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Grade_Subject_SubjectID",
                        column: x => x.SubjectID,
                        principalSchema: "School",
                        principalTable: "Subject",
                        principalColumn: "SubjectId",
                        onDelete: ReferentialAction.Restrict);
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
                    QuestionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TestId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    QuestionText = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Test", x => x.QuestionId);
                    table.ForeignKey(
                        name: "FK_Test_Test_TestId",
                        column: x => x.TestId,
                        principalSchema: "Test",
                        principalTable: "Test",
                        principalColumn: "TestID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Test",
                schema: "Answers",
                columns: table => new
                {
                    AnswerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    QuestionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AnswerText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Test", x => x.AnswerId);
                    table.ForeignKey(
                        name: "FK_Test_Test_QuestionId",
                        column: x => x.QuestionId,
                        principalSchema: "Questions",
                        principalTable: "Test",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Test",
                schema: "UserAnswers",
                columns: table => new
                {
                    UserAnswerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TestID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AnswerID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GradeID = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Class_Name",
                schema: "School",
                table: "Class",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ELearningClassUserInformations_TeachersAccountID",
                table: "ELearningClassUserInformations",
                column: "TeachersAccountID");

            migrationBuilder.CreateIndex(
                name: "IX_Grade_StudentID",
                schema: "School",
                table: "Grade",
                column: "StudentID");

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
                name: "IX_Lessons_ClassID",
                schema: "Lesson",
                table: "Lessons",
                column: "ClassID");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_SubjectID",
                schema: "Lesson",
                table: "Lessons",
                column: "SubjectID");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_TeacherID",
                schema: "Lesson",
                table: "Lessons",
                column: "TeacherID");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_LessonID",
                schema: "Lesson",
                table: "Materials",
                column: "LessonID");

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

            migrationBuilder.CreateIndex(
                name: "IX_Person_ClassID",
                schema: "Person",
                table: "Person",
                column: "ClassID");

            migrationBuilder.CreateIndex(
                name: "IX_Person_EmailAddress",
                schema: "Person",
                table: "Person",
                column: "EmailAddress",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Person_Surname",
                schema: "Person",
                table: "Person",
                column: "Surname");

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

            migrationBuilder.CreateIndex(
                name: "IX_Subject_ClassID",
                schema: "School",
                table: "Subject",
                column: "ClassID");

            migrationBuilder.CreateIndex(
                name: "IX_Subject_TeacherID",
                schema: "School",
                table: "Subject",
                column: "TeacherID");

            migrationBuilder.CreateIndex(
                name: "IX_Test_QuestionId",
                schema: "Answers",
                table: "Test",
                column: "QuestionId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Test_TeacherID",
                schema: "Test",
                table: "Test",
                column: "TeacherID");

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
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ELearningClassUserInformations");

            migrationBuilder.DropTable(
                name: "Grade",
                schema: "School");

            migrationBuilder.DropTable(
                name: "Materials",
                schema: "Lesson");

            migrationBuilder.DropTable(
                name: "Notification",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "StudentSubject",
                schema: "School");

            migrationBuilder.DropTable(
                name: "Test",
                schema: "UserAnswers");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "School",
                schema: "GradeDetails");

            migrationBuilder.DropTable(
                name: "Lessons",
                schema: "Lesson");

            migrationBuilder.DropTable(
                name: "Test",
                schema: "Answers");

            migrationBuilder.DropTable(
                name: "Test",
                schema: "Questions");

            migrationBuilder.DropTable(
                name: "Test",
                schema: "Test");

            migrationBuilder.DropTable(
                name: "Subject",
                schema: "School");

            migrationBuilder.DropTable(
                name: "Person",
                schema: "Person");

            migrationBuilder.DropTable(
                name: "Address",
                schema: "Person");

            migrationBuilder.DropTable(
                name: "Class",
                schema: "School");
        }
    }
}
