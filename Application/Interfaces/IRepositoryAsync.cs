using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
