using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NpgsqlTypes;

namespace DashboardApi
{
    public class DashboardContext : DbContext
    {
        public DashboardContext(DbContextOptions<DashboardContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");
                
            
            modelBuilder
                .Entity<Case>()
                .Property(e => e.CaseId)
                .HasDefaultValueSql("uuid_generate_v4()");

            modelBuilder
                .Entity<User>()
                .Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()");
        }

        public DbSet<Case> Cases { get; set; }
        public DbSet<User> Users { get; set; }
    }

    public class Case
    {
        [Key]
        public Guid CaseId { get; set; }
        [Required]
        public Guid EntityId { get; set; }
        [Required]
        public int Dead { get; set; }
        [Required]
        public int Infected { get; set; }
        [Required]
        public int Recovered { get; set; }
        [Required]
        public int IntensiveCare { get; set; }
        [Required]
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }
    }

    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string email { get; set; }
        public string passwordHash { get; set; }
    }
}