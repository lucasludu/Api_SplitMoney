using Application.Features._groups.DTOs;
using Application.Interfaces;
using Application.Specification._settlements;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Groups.Queries
{
    public class GetMySettlementsQueryHandler : IRequestHandler<GetMySettlementsQuery, Response<List<SettlementResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthenticatedUserService _userService;

        public GetMySettlementsQueryHandler(IUnitOfWork unitOfWork, IAuthenticatedUserService userService)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
        }

        public async Task<Response<List<SettlementResponse>>> Handle(GetMySettlementsQuery request, CancellationToken cancellationToken)
        {
            var userId = _userService.UserId;
            
            var spec = new MySettlementsSpecification(userId);
            var settlements = await _unitOfWork.RepositoryAsync<Settlement>().ListAsync(spec, cancellationToken);
            
            // To get PayeeNames efficiently, we can fetch users later or just use a simple lookup if needed.
            // For now let's fetch all relevant users.
            var payeeIds = settlements.Select(s => s.PayeeId).Distinct().ToList();
            var allUsers = await _unitOfWork.RepositoryAsync<ApplicationUser>().ListAsync();
            var payees = allUsers.Where(u => payeeIds.Contains(u.Id)).ToDictionary(u => u.Id, u => $"{u.FirstName} {u.LastName}".Trim());

            var result = settlements.Select(s => new SettlementResponse
            {
                Id = s.Id,
                GroupId = s.GroupId,
                GroupName = s.Group?.Name ?? "Grupo Desconocido",
                PayerId = s.PayerId,
                PayeeId = s.PayeeId,
                PayeeName = payees.ContainsKey(s.PayeeId) ? payees[s.PayeeId] : "Usuario Desconocido",
                Amount = s.Amount.Amount,
                Currency = s.Amount.Currency,
                Date = s.Date,
                ProofImageUrl = s.ProofImageUrl
            }).ToList();

            return new Response<List<SettlementResponse>>(result);
        }
    }
}
