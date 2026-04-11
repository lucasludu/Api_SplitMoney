using Application.Features._categories.DTOs;
using Application.Wrappers;
using MediatR;

namespace Application.Features._categories.Queries.GetAllCategories
{
    public record GetAllCategoriesQuery : IRequest<Response<List<CategoryDto>>> { }
}
