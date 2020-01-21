using System;
using fristrupminde_api_backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNet.Identity.EntityFramework;

namespace fristrupminde_api_backend.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<ProjectTask> Tasks { get; set; }
    }
}