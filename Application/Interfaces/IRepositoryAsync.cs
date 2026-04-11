using Ardalis.Specification;

namespace Application.Interfaces
{
    public interface IRepositoryAsync<T> : IRepositoryBase<T> where T : class
    {
        IQueryable<T> Entities { get; }
    }

    public interface IReadRepositoryAsync<T> : IReadRepositoryBase<T> where T : class 
    { 
    }

}
