using ELearning_Platform.Domain.Enitities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ELearning_Platform.Infrastructure.Database
{
    public class PlatformDb(DbContextOptions options) : IdentityDbContext<Account, Roles, string>(options)
    {
        public DbSet<UserAddress> UserAddresses { get; set; }   

        public DbSet<UserInformations> UserInformations {  get; set; }  

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
            base.OnModelCreating(builder);

        }
    }
}
