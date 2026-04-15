using Ardalis.Specification;
using Domain.Entities;

namespace Application.Specification._settlements
{
    public class MySettlementsSpecification : Specification<Settlement>
    {
        public MySettlementsSpecification(string payerId)
        {
            Query.Where(s => s.PayerId == payerId)
                 .Include(s => s.Group)
                 .OrderByDescending(s => s.Date);
        }
    }
}
