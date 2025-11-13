using Microsoft.EntityFrameworkCore;
using RiskManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiskManagementSystem.Data
{
    public class AppDbContext:DbContext
    {
    public DbSet<RiskRule> RiskRules {  get; set; }
    public DbSet<Claim> Claims {  get; set; }
        public DbSet<Customer> Customers { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=RiskManagementDb;Trusted_Connection=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<RiskRule>()
                .HasMany(r => r.Claims)
                .WithMany(r => r.RiskRules).
                UsingEntity(t => t.ToTable("ClaimRiskRules"));
            modelBuilder.Entity<Claim>()
                .HasOne(c => c.Customer)
                .WithMany(c => c.Claims)
                .HasForeignKey(c=>c.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
