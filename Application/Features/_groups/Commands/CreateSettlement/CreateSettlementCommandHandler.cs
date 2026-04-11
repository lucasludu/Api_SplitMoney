using Application.Interfaces;
using Application.Specification._user;
using Application.Wrappers;
using Domain.Common;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Groups.Commands
{
    public class CreateSettlementCommandHandler : IRequestHandler<CreateSettlementCommand, Response<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthenticatedUserService _authenticatedUser;

        public CreateSettlementCommandHandler(IUnitOfWork unitOfWork, IAuthenticatedUserService authenticatedUser)
        {
            _unitOfWork = unitOfWork;
            _authenticatedUser = authenticatedUser;
        }

        public async Task<Response<Guid>> Handle(CreateSettlementCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;
            var userId = _authenticatedUser.UserId;

            // Validar que el pagador pertenece al grupo (Specification)
            var payerSpec = new MemberInGroupSpecification(request.GroupId, userId);
            var payerMember = await _unitOfWork.RepositoryAsync<GroupMember>()
                .FirstOrDefaultAsync(payerSpec, cancellationToken);
            
            if (payerMember == null)
                return Response<Guid>.Fail("El pagador no pertenece al grupo especificado.");

            // Validar que el cobrador pertenece al grupo (Specification)
            var payeeSpec = new MemberInGroupSpecification(request.GroupId, request.PayeeId);
            var payeeMember = await _unitOfWork.RepositoryAsync<GroupMember>()
                .FirstOrDefaultAsync(payeeSpec, cancellationToken);
            
            if (payeeMember == null)
                return Response<Guid>.Fail("El destinatario del pago no pertenece al grupo especificado.");

            var settlement = new Settlement
            {
                GroupId = request.GroupId,
                PayerId = userId, 
                PayeeId = request.PayeeId,
                Amount = new Money(request.Amount, request.Currency),
                Date = request.Date,
                ProofImageUrl = request.ProofImageUrl
            };

            await _unitOfWork.RepositoryAsync<Settlement>().AddAsync(settlement);
            await _unitOfWork.SaveChangesAsync();

            return new Response<Guid>(settlement.Id, "Liquidación registrada correctamente.");
        }
    }
}
