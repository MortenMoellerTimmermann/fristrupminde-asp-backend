﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using fristrupminde_api.Data;

namespace fristrupminde_api.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity("fristrupminde_api.Models.Animal", b =>
                {
                    b.Property<string>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Born");

                    b.Property<DateTime?>("Death");

                    b.HasKey("ID");

                    b.ToTable("Animals");
                });

            modelBuilder.Entity("fristrupminde_api.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp");

                    b.Property<string>("Email");

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail");

                    b.Property<string>("NormalizedUserName");

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.ToTable("ApplicationUsers");
                });

            modelBuilder.Entity("fristrupminde_api.Models.ProjectTask", b =>
                {
                    b.Property<string>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<string>("Description");

                    b.Property<DateTime>("DueDate");

                    b.Property<string>("Title");

                    b.HasKey("ID");

                    b.ToTable("ProjectTasks");
                });

            modelBuilder.Entity("fristrupminde_api.Models.ProjectTaskUser", b =>
                {
                    b.Property<string>("ProjectTaskUserID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ProjectTaskID");

                    b.Property<string>("UserID");

                    b.HasKey("ProjectTaskUserID");

                    b.ToTable("ProjectTaskUsers");
                });
#pragma warning restore 612, 618
        }
    }
}
