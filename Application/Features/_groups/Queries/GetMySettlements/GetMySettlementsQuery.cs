using Application.Wrappers;
using Application.Features._groups.DTOs;
using MediatR;
using System.Collections.Generic;

namespace Application.Features.Groups.Queries
{
    public class GetMySettlementsQuery : IRequest<Response<List<SettlementResponse>>>
    {
    }
}
