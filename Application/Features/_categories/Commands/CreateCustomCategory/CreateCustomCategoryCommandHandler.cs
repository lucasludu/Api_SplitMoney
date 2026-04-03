using Application.Constants;
using Application.Exceptions;
using Application.Interfaces;
using Application.Wrappers;
using Domain.Entities;
using MediatR;

namespace Application.Features._categories.Commands.CreateCustomCategory
{
    public class CreateCustomCategoryCommandHandler : IRequestHandler<CreateCustomCategoryCommand, Response<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthenticatedUserService _authenticatedUser;

        public CreateCustomCategoryCommandHandler(IUnitOfWork unitOfWork, IAuthenticatedUserService authenticatedUser)
        {
            _unitOfWork = unitOfWork;
            _authenticatedUser = authenticatedUser;
        }

        public async Task<Response<Guid>> Handle(CreateCustomCategoryCommand request, CancellationToken cancellationToken)
        {
            var isPremium = _authenticatedUser.Roles.Contains(RolesConstants.PremiumUser);
            if (!isPremium)
            {
                throw new ApiException("Esta característica es exclusiva para usuarios Premium.");
            }

            var category = new Category
            {
                Name = request.Name,
                IconIdentifier = request.IconIdentifier,
                ColorHex = request.ColorHex,
                ApplicationUserId = _authenticatedUser.UserId
            };

            var newCategory = await _unitOfWork.RepositoryAsync<Category>().AddAsync(category);
            await _unitOfWork.SaveChangesAsync();

            return new Response<Guid>(newCategory.Id, "Categoría personalizada creada.");
        }
    }
}
