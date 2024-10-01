using ELearning_Platform.Domain.Enitities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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

        //Lesson
        public DbSet<Lesson> Lessons { get; set; }

        public DbSet<LessonMaterials> LessonMaterials { get; set; }

        //Account
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<Account>()
                .ToTable("Account", "Person");

            builder.Entity<Roles>()
                .ToTable("Roles", "Person");

            builder.Entity<UserInformations>(options =>
            {
                options.ToTable("Person", "Person");
                options.HasKey(pr => pr.AccountID);
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

            base.OnModelCreating(builder);

        }
    }
}
