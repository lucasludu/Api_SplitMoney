using Application.Wrappers;
using MediatR;

namespace Application.Features._groups.Queries.GetGroupsByUser
{
    public class GetGroupsByUserQuery : IRequest<Response<IEnumerable<GroupDto>>>
    {
        public string UserId { get; set; } = string.Empty;
    }

    public class GroupDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int MembersCount { get; set; }
        public decimal TotalBalance { get; set; } // Mocked for now
    }
}
