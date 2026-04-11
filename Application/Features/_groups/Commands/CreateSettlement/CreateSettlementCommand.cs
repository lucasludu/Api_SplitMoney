using Application.Features._groups.DTOs;
using Application.Wrappers;
using MediatR;

namespace Application.Features.Groups.Commands
{
    public record CreateSettlementCommand(CreateSettlementRequest Request) : IRequest<Response<Guid>>;
}
