using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Application.Interfaces
{
    public interface IApplicationDbContext
    {
        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        DatabaseFacade Database { get; }

        Microsoft.EntityFrameworkCore.DbSet<Domain.Entities.Group> Groups { get; }
        Microsoft.EntityFrameworkCore.DbSet<Domain.Entities.GroupMember> GroupMembers { get; }
        Microsoft.EntityFrameworkCore.DbSet<Domain.Entities.Expense> Expenses { get; }
        Microsoft.EntityFrameworkCore.DbSet<Domain.Entities.ExpenseSplit> ExpenseSplits { get; }
        Microsoft.EntityFrameworkCore.DbSet<Domain.Entities.Balance> Balances { get; }
        Microsoft.EntityFrameworkCore.DbSet<Domain.Entities.Settlement> Settlements { get; }
        Microsoft.EntityFrameworkCore.DbSet<Domain.Entities.Category> Categories { get; }
        Microsoft.EntityFrameworkCore.DbSet<Domain.Entities.ExpenseAudit> ExpenseAudits { get; }
    }
}
