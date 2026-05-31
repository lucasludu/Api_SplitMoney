using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class ExpensePaymentConfiguration : IEntityTypeConfiguration<ExpensePayment>
    {
        public void Configure(EntityTypeBuilder<ExpensePayment> builder)
        {
            builder.HasOne(ep => ep.Expense)
                .WithMany(e => e.Payments)
                .HasForeignKey(ep => ep.ExpenseId);

            builder.HasOne(ep => ep.User)
                .WithMany()
                .HasForeignKey(ep => ep.UserId);
        }
    }
}
