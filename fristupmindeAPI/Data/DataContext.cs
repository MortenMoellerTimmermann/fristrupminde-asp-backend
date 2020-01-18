using System;
using fristupmindeAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


namespace fristupmindeAPI.Data
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        public DataContext(DbContextOptions<DataContext> options) :base(options)
        {
        }

        public DbSet<ProjectTask> Tasks { get; set; }

        /*
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>(entity => {
                // Due to some InnoDb's byte restriction we specify the coloums size 
                // to avoid migration errors
                entity.ToTable(name: "Users");
                entity.Property(c => c.NormalizedEmail)
                    .HasMaxLength(128)
                    .IsRequired();
                entity.Property(c => c.NormalizedUserName)
                    .HasMaxLength(128)
                    .IsRequired();
            });


            builder.Entity<IdentityUserClaim<int>>(entity => entity.ToTable(name: "UserClaims"));
            builder.Entity<IdentityUserLogin<int>>(entity => entity.ToTable(name: "UserLogins"));
            builder.Entity<IdentityRoleClaim<int>>(entity => entity.ToTable(name: "RoleClaims"));
            builder.Entity<IdentityUserToken<int>>(entity => entity.ToTable(name: "UserTokens"));
        } */
    }
}
