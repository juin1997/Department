using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Department.Models;

namespace Department.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Depart> Departs { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<DtoMMapping> DtoMMappings { get; set; }
        public DbSet<DtoAMapping> DtoAMappings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            modelBuilder.Entity<Depart>().HasKey(d => d.ID);
            modelBuilder.Entity<Depart>().HasIndex(d => d.Name);
            modelBuilder.Entity<Depart>().HasIndex(d => d.Email);

            modelBuilder.Entity<Student>().HasKey(s => s.ID);
            modelBuilder.Entity<Student>().HasIndex(s => s.Name);
            modelBuilder.Entity<Student>().HasIndex(s => s.Email);

            modelBuilder.Entity<Activity>().HasKey(a => a.ID);
            modelBuilder.Entity<Activity>().HasIndex(a => a.DepartID);

            modelBuilder.Entity<Application>().HasKey(a => a.ID);
            modelBuilder.Entity<Application>().HasIndex(a => a.DepartID);
            modelBuilder.Entity<Application>().HasIndex(a => a.Grade);
            modelBuilder.Entity<Application>().HasIndex(a => a.Institute);

            modelBuilder.Entity<DtoMMapping>().HasKey(m => m.ID);
            modelBuilder.Entity<DtoMMapping>().HasIndex(m => m.DepartID);
            modelBuilder.Entity<DtoMMapping>().HasIndex(m => m.MemberID);

            modelBuilder.Entity<DtoAMapping>().HasKey(m => m.ID);
            modelBuilder.Entity<DtoAMapping>().HasIndex(m => m.DepartID);
            modelBuilder.Entity<DtoAMapping>().HasIndex(m => m.ApplicationID);
            modelBuilder.Entity<DtoAMapping>().HasIndex(m => m.StudentID);
        }
    }
}
