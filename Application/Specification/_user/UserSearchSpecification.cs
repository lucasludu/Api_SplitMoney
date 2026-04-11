using Ardalis.Specification;
using Domain.Entities;

namespace Application.Specification._user
{
    public class UserSearchSpecification : Specification<ApplicationUser>
    {
        public UserSearchSpecification(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                Query.Take(10); // Default to first 10 if no term
            }
            else
            {
                var lowerTerm = term.ToLower();
                Query.Where(u => u.Email!.ToLower().Contains(lowerTerm) 
                              || u.FirstName.ToLower().Contains(lowerTerm) 
                              || u.LastName.ToLower().Contains(lowerTerm))
                     .Take(20);
            }
        }
    }
}
