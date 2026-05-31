using Application.Features._groups.DTOs;
using Application.Wrappers;
using MediatR;

namespace Application.Features.Groups.Queries
{
    public class GetSimplifiedDebtsQuery : IRequest<Response<List<SimplifiedDebtDto>>>
    {
        public Guid GroupId { get; set; }

        public GetSimplifiedDebtsQuery(Guid groupId)
        {
            GroupId = groupId;
        }
    }
}
