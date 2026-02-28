using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Application.Interfaces
{
    public interface IApplicationDbContext
    {
        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        DatabaseFacade Database { get; }
    }
}
