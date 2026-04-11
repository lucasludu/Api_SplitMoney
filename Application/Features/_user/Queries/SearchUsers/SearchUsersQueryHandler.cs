using Application.Features._user.DTOs;
using Application.Interfaces;
using Application.Specification._user;
using Application.Wrappers;
using Domain.Entities;
using MediatR;

namespace Application.Features._user.Queries.SearchUsers
{
    public class SearchUsersQueryHandler : IRequestHandler<SearchUsersQuery, Response<List<UserResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SearchUsersQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<List<UserResponse>>> Handle(SearchUsersQuery request, CancellationToken cancellationToken)
        {
            var spec = new UserSearchSpecification(request.Term);
            var users = await _unitOfWork.RepositoryAsync<ApplicationUser>().ListAsync(spec, cancellationToken);

            var response = users.Select(u => new UserResponse
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email!
            }).ToList();

            return new Response<List<UserResponse>>(response);
        }
    }
}
