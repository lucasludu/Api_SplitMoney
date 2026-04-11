using Application.Interfaces;
using Persistence.Contexts;
using Persistence.Repository;
using System.Collections;

namespace Persistence.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private Hashtable? _repositories;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IRepositoryAsync<T> RepositoryAsync<T>() where T : class
        {
            if (_repositories == null)
                _repositories = new Hashtable();

            var type = typeof(T).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(MyRepositoryAsync<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _context);
                _repositories.Add(type, repositoryInstance);
            }

            return (IRepositoryAsync<T>)_repositories[type]!;
        }

        public IReadRepositoryAsync<T> ReadRepositoryAsync<T>() where T : class
        {
            if (_repositories == null)
                _repositories = new Hashtable();

            var type = typeof(T).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(MyRepositoryAsync<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _context);
                _repositories.Add(type, repositoryInstance);
            }

            return (IReadRepositoryAsync<T>)_repositories[type]!;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public Task Rollback()
        {
            return Task.CompletedTask; // Todo: Manejo mas complejo si es necesario
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
