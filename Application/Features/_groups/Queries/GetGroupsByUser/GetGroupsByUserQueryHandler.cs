using Application.Interfaces;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using Ardalis.Specification;

namespace Application.Features.Groups.Queries
{
    public class GetGroupsByUserQueryHandler : IRequestHandler<GetGroupsByUserQuery, Response<IEnumerable<GroupDto>>>
    {
        private readonly IRepositoryAsync<Group> _repository;

        public GetGroupsByUserQueryHandler(IRepositoryAsync<Group> repository)
        {
            _repository = repository;
        }

        public async Task<Response<IEnumerable<GroupDto>>> Handle(GetGroupsByUserQuery request, CancellationToken cancellationToken)
        {
            // Simple specification to get groups where user is a member
            var groups = await _repository.ListAsync(new GroupsByUserSpec(request.UserId), cancellationToken);

            var groupDtos = groups.Select(g => new GroupDto
            {
                Id = g.Id,
                Name = g.Name,
                Description = g.Description,
                MembersCount = g.Members.Count,
                TotalBalance = 0 // Mocked for now, would use BalanceEngineService
            });

            return new Response<IEnumerable<GroupDto>>(groupDtos);
        }
    }

    public class GroupsByUserSpec : Specification<Group>
    {
        public GroupsByUserSpec(string userId)
        {
            Query.Where(g => g.Members.Any(m => m.UserId == userId))
                 .Include(g => g.Members);
        }
    }
}
