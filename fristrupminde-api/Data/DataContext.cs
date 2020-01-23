using System;
using fristrupminde_api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNet.Identity.EntityFramework;

namespace fristrupminde_api.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<ProjectTask> ProjectTasks { get; set; }
        public DbSet<Animal> Animals { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ProjectTaskUser> ProjectTaskUsers { get; set; }

        //https://github.com/dotnet/efcore/issues/17788
    }
}
