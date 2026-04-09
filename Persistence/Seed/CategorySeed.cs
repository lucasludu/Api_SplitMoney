using Domain.Entities;
using Persistence.Contexts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Seed
{
    public static class CategorySeed
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

            if (!await dbContext.Categories.AnyAsync(c => c.ApplicationUserId == null))
            {
                var defaultCategories = new List<Category>
                {
                    new Category { Name = "General", IconIdentifier = "💰", ColorHex = "#9CA3AF" },
                    new Category { Name = "Comida", IconIdentifier = "🍔", ColorHex = "#F59E0B" },
                    new Category { Name = "Transporte", IconIdentifier = "🚗", ColorHex = "#3B82F6" },
                    new Category { Name = "Entretenimiento", IconIdentifier = "🍿", ColorHex = "#8B5CF6" },
                    new Category { Name = "Supermercado", IconIdentifier = "🛒", ColorHex = "#10B981" },
                    new Category { Name = "Viajes", IconIdentifier = "✈️", ColorHex = "#06B6D4" },
                    new Category { Name = "Alojamiento", IconIdentifier = "🏨", ColorHex = "#F43F5E" }
                };

                await dbContext.Categories.AddRangeAsync(defaultCategories);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
