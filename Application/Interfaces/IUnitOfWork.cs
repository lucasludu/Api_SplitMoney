namespace Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepositoryAsync<T> RepositoryAsync<T>() where T : class;
        IReadRepositoryAsync<T> ReadRepositoryAsync<T>() where T : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task Rollback();
    }
}
