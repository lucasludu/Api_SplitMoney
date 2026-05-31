using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class BalanceConfiguration : IEntityTypeConfiguration<Balance>
    {
        public void Configure(EntityTypeBuilder<Balance> builder)
        {
            builder.HasOne(b => b.Group)
                .WithMany(g => g.Balances)
                .HasForeignKey(b => b.GroupId);

            builder.HasOne(b => b.Debtor)
                .WithMany()
                .HasForeignKey(b => b.DebtorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.Creditor)
                .WithMany()
                .HasForeignKey(b => b.CreditorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(b => b.Currency).HasMaxLength(3).IsRequired();
        }
    }
}
