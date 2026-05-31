using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class ExpenseAuditConfiguration : IEntityTypeConfiguration<ExpenseAudit>
    {
        public void Configure(EntityTypeBuilder<ExpenseAudit> builder)
        {
            builder.HasOne(ea => ea.Expense)
                .WithMany()
                .HasForeignKey(ea => ea.ExpenseId);

            builder.HasOne(ea => ea.ModifiedByUser)
                .WithMany()
                .HasForeignKey(ea => ea.ModifiedByUserId);

            builder.Property(ea => ea.Action).IsRequired().HasMaxLength(100);
        }
    }
}
