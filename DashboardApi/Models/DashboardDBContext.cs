using Microsoft.EntityFrameworkCore;
using NpgsqlTypes;

namespace DashboardApi
{
    public class DashboardDBContext : DbContext
    {
        public DashboardDBContext(DbContextOptions<DashboardDBContext> options) : base(options)
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
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}