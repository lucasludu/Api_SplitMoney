using Application.Interfaces;
using Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

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

    public class GetGroupMembersQueryHandler : IRequestHandler<GetGroupMembersQuery, Response<List<GroupMemberResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetGroupMembersQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<List<GroupMemberResponse>>> Handle(GetGroupMembersQuery request, CancellationToken cancellationToken)
        {
            var members = await _unitOfWork.RepositoryAsync<GroupMember>()
                .Entities
                .Include(gm => gm.User)
                .Where(gm => gm.GroupId == request.GroupId)
                .Select(gm => new GroupMemberResponse
                {
                    UserId = gm.UserId,
                    FullName = $"{gm.User.FirstName} {gm.User.LastName}",
                    Email = gm.User.Email!
                })
                .ToListAsync(cancellationToken);

            return new Response<List<GroupMemberResponse>>(members);
        }
    }
}
