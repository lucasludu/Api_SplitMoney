using Application.Interfaces;
using Domain.Common;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Persistence.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>, IApplicationDbContext
    {
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupMember> GroupMembers { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<ExpenseSplit> ExpenseSplits { get; set; }
        public DbSet<ExpensePayment> ExpensePayments { get; set; }
        public DbSet<Balance> Balances { get; set; }
        public DbSet<Settlement> Settlements { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<ExpenseAudit> ExpenseAudits { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        private readonly Persistence.Interceptors.AuditableEntityInterceptor? _auditableEntityInterceptor;

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            Persistence.Interceptors.AuditableEntityInterceptor? auditableEntityInterceptor = null)
        : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            _auditableEntityInterceptor = auditableEntityInterceptor;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_auditableEntityInterceptor != null)
            {
                optionsBuilder.AddInterceptors(_auditableEntityInterceptor);
            }
            base.OnConfiguring(optionsBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 
            
            // Decimal to double conversion for SQLite
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetValueConverter(new Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<decimal, double>(
                    v => (double)v,
                    v => (decimal)v
                ));
            }

            modelBuilder.Entity<Expense>(entity =>
            {
                entity.OwnsOne(e => e.Amount, a =>
                {
                    a.Property(p => p.Amount).HasColumnName("TotalAmount");
                    a.Property(p => p.Currency).HasColumnName("Currency").HasMaxLength(3);
                });
            });

            modelBuilder.Entity<Settlement>(entity =>
            {
                entity.OwnsOne(s => s.Amount, a =>
                {
                    a.Property(p => p.Amount).HasColumnName("Amount");
                    a.Property(p => p.Currency).HasColumnName("Currency").HasMaxLength(3);
                });
            });

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
