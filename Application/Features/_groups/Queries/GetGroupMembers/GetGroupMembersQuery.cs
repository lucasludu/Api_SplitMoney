using Application.Wrappers;
using MediatR;

namespace Application.Features.Groups.Queries
{
    public class GetGroupMembersQuery : IRequest<Response<List<GroupMemberResponse>>>
    {
        public Guid GroupId { get; set; }
    }

    public class GroupMemberResponse
    {
        public string UserId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
