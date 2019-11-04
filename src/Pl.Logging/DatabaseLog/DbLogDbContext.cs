using Microsoft.EntityFrameworkCore;
using Pl.Core.Entities;
using Pl.Core.Security;
using Pl.EntityFrameworkCore;

namespace Pl.Logging
{
    public class DbLogDbContext : DbContext
    {
        public DbLogDbContext(DbContextOptions<DbLogDbContext> options) : base(options)
        {

        }

        public virtual DbSet<SystemLog> SystemLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<SystemValue>(b =>
            {
                b.Property(c => c.Id).HasMaxLength(36);
                b.Property(p => p.CreatedTime).ValueGeneratedOnAdd();
                b.Property(p => p.UpdatedTime).ValueGeneratedOnAddOrUpdate();
                b.Property(p => p.Key).HasMaxLength(128).IsRequired();
                b.Property(p => p.Value).HasConversion(new DataProtectedConverter(SystemDataProtection.DataProtector));
                b.HasIndex(q => q.Key);
            });

        }
    }
}