using Application.Features._categories.DTOs;
using Application.Interfaces;
using Application.Specification._categories;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features._categories.Queries.GetAllCategories
{
    public class GetAllCategoriesQuery : IRequest<Response<List<CategoryDto>>> { }

    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, Response<List<CategoryDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthenticatedUserService _authenticatedUser;

        public GetAllCategoriesQueryHandler(IUnitOfWork unitOfWork, IAuthenticatedUserService authenticatedUser)
        {
            _unitOfWork = unitOfWork;
            _authenticatedUser = authenticatedUser;
        }

        public async Task<Response<List<CategoryDto>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var userId = _authenticatedUser.UserId;
            var spec = new ActiveCategoriesSpecification(userId);

            // Obtener lista de categorías desde el repositorio
            var categories = await _unitOfWork.RepositoryAsync<Category>().ListAsync(spec, cancellationToken);

            // Mapear manualmente a DTO para mayor control (o usar AutoMapper si lo tienes)
            var categoryDtos = categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                IconIdentifier = c.IconIdentifier,
                ColorHex = c.ColorHex,
                IsGlobal = c.ApplicationUserId == null
            }).ToList();

            return new Response<List<CategoryDto>>(categoryDtos);
        }
    }
}
