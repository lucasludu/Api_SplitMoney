using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class SettlementConfiguration : IEntityTypeConfiguration<Settlement>
    {
        public void Configure(EntityTypeBuilder<Settlement> builder)
        {
            builder.OwnsOne(s => s.Amount, a =>
            {
                a.Property(p => p.Amount).HasColumnName("Amount");
                a.Property(p => p.Currency).HasColumnName("Currency").HasMaxLength(3);
            });

            builder.HasOne(s => s.Group)
                .WithMany(g => g.Settlements)
                .HasForeignKey(s => s.GroupId);

            builder.HasOne(s => s.Payer)
                .WithMany()
                .HasForeignKey(s => s.PayerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Payee)
                .WithMany()
                .HasForeignKey(s => s.PayeeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
