using Application.Features._user.DTOs;
using Application.Interfaces;
using Application.Wrappers;
using Domain.Entities;
using MediatR;

namespace Application.Features._user.Queries.GetMyProfile
{
    public class GetMyProfileQueryHandler : IRequestHandler<GetMyProfileQuery, Response<UserResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthenticatedUserService _authenticatedUser;

        public GetMyProfileQueryHandler(IUnitOfWork unitOfWork, IAuthenticatedUserService authenticatedUser)
        {
            _unitOfWork = unitOfWork;
            _authenticatedUser = authenticatedUser;
        }

        public async Task<Response<UserResponse>> Handle(GetMyProfileQuery request, CancellationToken cancellationToken)
        {
            var userId = _authenticatedUser.UserId;
            var user = await _unitOfWork.RepositoryAsync<ApplicationUser>().GetByIdAsync(userId);

            if (user == null)
                return new Response<UserResponse>("Usuario no encontrado en la sesión actual.");

            var response = new UserResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email!,
                PhoneNumber = user.PhoneNumber
            };

            return new Response<UserResponse>(response);
        }
    }
}
