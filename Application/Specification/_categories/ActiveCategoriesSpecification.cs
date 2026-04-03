using Ardalis.Specification;
using Domain.Entities;

namespace Application.Specification._categories
{
    public class ActiveCategoriesSpecification : Specification<Category>
    {
        public ActiveCategoriesSpecification(string? userId)
        {
            // El usuario ve: Categorías Globales (UserId == null) 
            // O sus categorías propias (UserId == userId)
            // Y siempre que estén activas (IsActive == true)
            Query.Where(c => c.IsActive && (c.ApplicationUserId == null || c.ApplicationUserId == userId))
                 .OrderBy(c => c.Name);
        }
    }
}
