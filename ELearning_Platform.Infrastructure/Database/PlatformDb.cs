using ELearning_Platform.Domain.Enitities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ELearning_Platform.Infrastructure.Database
{
    public class PlatformDb(DbContextOptions options) : IdentityDbContext<Account, Roles, string>(options)
    {
        //Person
        public DbSet<UserAddress> UserAddresses { get; set; }

        public DbSet<UserInformations> UserInformations { get; set; }

        //School
        public DbSet<Subject> Subjects { get; set; }

        public DbSet<ELearningClass> ELearningClasses { get; set; }

        public DbSet<Grade> Grades { get; set; }    //!!!!

        //Lesson
        public DbSet<Lesson> Lessons { get; set; }

        public DbSet<LessonMaterials> LessonMaterials { get; set; }

        //Account
        public DbSet<Notification> Notifications { get; set; }


        //Test

        public DbSet<Test> Tests { get; set; } //!!!!

        public DbSet<Questions> Questions { get; set; } //!!!!

        public DbSet<Answers> Answers { get; set; } //!!!!
        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<Account>()
                .ToTable("Account", "Person");

            builder.Entity<Roles>()
                .ToTable("Roles", "Person");

            builder.Entity<UserInformations>(options =>
            {
                options.ToTable("Person", "Person");
                //Key
                options.HasKey(pr => pr.AccountID);
                
                //Indexes
                options.HasIndex(pr=>pr.Surname);
                options.HasIndex(pr => pr.EmailAddress).IsUnique();

                //Relations
                options.HasOne(pr => pr.Account)
                .WithOne(pr => pr.User)
                .HasForeignKey<Account>(pr => pr.Id)
                .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<UserAddress>(options =>
            {
                options.HasKey(pr => pr.AccountID);

                options.ToTable("Address", "Person");
                options.HasOne(pr => pr.User)
                .WithOne(pr => pr.Address)
                .HasForeignKey<UserInformations>(pr => pr.AccountID)
                .OnDelete(DeleteBehavior.Restrict);
            });


            builder.Entity<Subject>(options =>
            {
                options.ToTable("Subject", "School");
                options.HasOne(pr => pr.Teacher)
                .WithMany(pr => pr.Subjects)
                .HasForeignKey(pr => pr.TeacherID);

                options.HasOne(pr => pr.Class)
                .WithMany(pr => pr.Subjects)
                .HasForeignKey(pr => pr.ClassID);

            });

            builder.Entity<ELearningClass>(options =>
            {
                options.ToTable("Class", "School");

                options.HasIndex(pr => pr.Name).IsUnique();

                options.HasMany(pr => pr.Students)
                .WithOne(pr => pr.Class)
                .HasForeignKey(pr => pr.ClassID);

                options.HasMany(pr => pr.Teachers)
                .WithMany(pr => pr.ListOfTeachingClasses);
            });

            builder.Entity<Lesson>(options =>
            {
                options.ToTable("Lessons", "Lesson");

                options.HasOne(pr => pr.Teacher)
                .WithMany(pr => pr.Lessons)
                .HasForeignKey(pr => pr.TeacherID);

                options.HasOne(pr => pr.Class)
                .WithMany(pr => pr.Lessons)
                .HasForeignKey(pr => pr.ClassID);

                options.HasOne(pr=>pr.Subject)
                .WithMany(pr=>pr.Lessons)
                .HasForeignKey(pr=>pr.SubjectID)
                .OnDelete(DeleteBehavior.Restrict);

            });

            builder.Entity<LessonMaterials>(options =>
            {
                options.ToTable("Materials", "Lesson");


                options.HasKey(pr => pr.LessonMaterialID);
                options.HasOne(pr => pr.Lesson)
                .WithMany(pr => pr.LessonMaterials)
                .HasForeignKey(pr => pr.LessonID);
            });

            builder.Entity<Notification>(options => 
            {
                options.ToTable("Notification", "Account");

                options.HasKey(pr=>pr.NotficaitonID);
                options.HasOne(pr=>pr.Sender)
                .WithMany(pr=>pr.SentNotfications)
                .HasForeignKey(pr=>pr.SenderID)
                .OnDelete(DeleteBehavior.Restrict);
                
                options.HasOne(pr => pr.Recipient)
                .WithMany(pr => pr.RecivedNotifications)
                .HasForeignKey(pr => pr.RecipientID)
                .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Grade>(options => 
            {
                options.ToTable("Grade", "School");

                options.HasKey(pr => pr.GradeID);

                options.HasIndex(pr=>pr.GradeLevel);

                options.HasOne(pr => pr.Subject)
                .WithMany(pr => pr.Grades)
                .HasForeignKey(pr => pr.SubjectID)
                .OnDelete(DeleteBehavior.Restrict);

                options.HasOne(pr=>pr.Account)
                .WithMany(pr=>pr.Grades)
                .HasForeignKey(pr=>pr.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

                options.HasOne(pr=>pr.Test)
                .WithMany(pr=>pr.Grades)
                .HasForeignKey(pr=>pr.TestID)
                .OnDelete(DeleteBehavior.Restrict);

            });

            builder.Entity<Test>(options => 
            {
                options.ToTable("Test", "Test");

                options.HasKey(pr=>pr.TestID);

                options.HasOne(pr=>pr.Subject)
                .WithMany(pr=>pr.Tests)
                .HasForeignKey(pr=>pr.SubjectID)
                .OnDelete(DeleteBehavior.Restrict);
            });


            builder.Entity<Questions>(options =>
            {
                options.ToTable("Test", "Questions");

                options.HasKey(pr => pr.QuestionId);

                options.HasOne(pr => pr.Test)
                .WithMany(pr => pr.Questions)
                .HasForeignKey(pr => pr.TestId)
                .OnDelete(DeleteBehavior.Restrict);

               
            });

            builder.Entity<Answers>(options =>
            {
                options.ToTable("Test", "Answers");

                options.HasKey(pr => pr.AnswerId);

                options.HasOne(pr => pr.Questions)
                .WithMany(pr => pr.Answers)
                .HasForeignKey(pr => pr.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);

            });
            base.OnModelCreating(builder);

        }
    }
}
