using System;
using fristrupminde_api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace fristrupminde_api.Data
{
    public class DataContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<ProjectTask> ProjectTasks { get; set; }
        public DbSet<Animal> Animals { get; set; }
        public DbSet<ProjectTaskUser> ProjectTaskUsers { get; set; }
        public DbSet<StatisticsData> StatisticsDatas { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>(entity => {
                entity.ToTable(name: "Users");
                entity.Property(c => c.NormalizedEmail)
                    .HasMaxLength(128)
                    .IsRequired();
                entity.Property(c => c.NormalizedUserName)
                    .HasMaxLength(128)
                    .IsRequired();
                entity.Property(c => c.EmailConfirmed).HasConversion<int>();
                entity.Property(c => c.PhoneNumberConfirmed).HasConversion<int>();
                entity.Property(c => c.TwoFactorEnabled).HasConversion<int>();
                entity.Property(c => c.LockoutEnabled).HasConversion<int>();
            });
            builder.Entity<ApplicationRole>(entity => {
                entity.ToTable(name: "Role");
                entity.Property(c => c.NormalizedName)
                    .HasMaxLength(128)
                    .IsRequired();
            });

            builder.Entity<IdentityUserLogin<Guid>>(entity => entity.Property(m => m.LoginProvider).HasMaxLength(85));
            builder.Entity<IdentityUserLogin<Guid>>(entity => entity.Property(m => m.ProviderKey).HasMaxLength(85));
            builder.Entity<IdentityUserLogin<Guid>>(entity => entity.Property(m => m.UserId).HasMaxLength(85));
            builder.Entity<IdentityUserRole<Guid>>(entity => entity.Property(m => m.UserId).HasMaxLength(85));
            builder.Entity<IdentityUserRole<Guid>>(entity => entity.Property(m => m.RoleId).HasMaxLength(85));
            builder.Entity<IdentityUserToken<Guid>>(entity => entity.Property(m => m.UserId).HasMaxLength(85));
            builder.Entity<IdentityUserToken<Guid>>(entity => entity.Property(m => m.LoginProvider).HasMaxLength(85));
            builder.Entity<IdentityUserToken<Guid>>(entity => entity.Property(m => m.Name).HasMaxLength(85));
            builder.Entity<IdentityUserClaim<Guid>>(entity => entity.Property(m => m.Id).HasMaxLength(85));
            builder.Entity<IdentityUserClaim<Guid>>(entity => entity.Property(m => m.UserId).HasMaxLength(85));
            builder.Entity<IdentityRoleClaim<Guid>>(entity => entity.Property(m => m.Id).HasMaxLength(85));
            builder.Entity<IdentityRoleClaim<Guid>>(entity => entity.Property(m => m.RoleId).HasMaxLength(85));
        }
    }
}
