using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Features._groups.DTOs;
using Application.Features.Groups.Queries;
using Application.Interfaces;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Groups.Queries
{
    public class GetSimplifiedDebtsQueryHandler : IRequestHandler<GetSimplifiedDebtsQuery, Response<List<SimplifiedDebtDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetSimplifiedDebtsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<List<SimplifiedDebtDto>>> Handle(GetSimplifiedDebtsQuery request, CancellationToken cancellationToken)
        {
            // 1. Fetch group members
            var members = await _unitOfWork.RepositoryAsync<GroupMember>()
                .Entities
                .Include(gm => gm.User)
                .Where(gm => gm.GroupId == request.GroupId)
                .ToListAsync(cancellationToken);

            if (!members.Any())
            {
                return new Response<List<SimplifiedDebtDto>>("Group not found or has no members.");
            }

            // 2. Fetch all expense payments in the group
            var payments = await _unitOfWork.RepositoryAsync<ExpensePayment>()
                .Entities
                .Include(ep => ep.Expense)
                .Where(ep => ep.Expense.GroupId == request.GroupId)
                .ToListAsync(cancellationToken);

            // 3. Fetch all expense splits in the group
            var splits = await _unitOfWork.RepositoryAsync<ExpenseSplit>()
                .Entities
                .Include(es => es.Expense)
                .Where(es => es.Expense.GroupId == request.GroupId)
                .ToListAsync(cancellationToken);

            // 4. Fetch all settlements in the group
            var settlements = await _unitOfWork.RepositoryAsync<Settlement>()
                .Entities
                .Where(s => s.GroupId == request.GroupId)
                .ToListAsync(cancellationToken);

            // 5. Calculate net balance for each member
            var memberBalances = new List<MemberBalance>();

            foreach (var member in members)
            {
                var userId = member.UserId;
                var fullName = $"{member.User.FirstName} {member.User.LastName}";

                // Paid toward expenses
                var totalPaid = payments.Where(p => p.UserId == userId).Sum(p => p.AmountPaid);

                // Owed for expenses
                var totalOwed = splits.Where(s => s.UserId == userId).Sum(s => s.AmountOwed);

                // Settlements sent (paid to others)
                var totalSent = settlements.Where(s => s.PayerId == userId).Sum(s => s.Amount.Amount);

                // Settlements received (collected from others)
                var totalReceived = settlements.Where(s => s.PayeeId == userId).Sum(s => s.Amount.Amount);

                var netBalance = (totalPaid + totalSent) - (totalOwed + totalReceived);

                memberBalances.Add(new MemberBalance
                {
                    UserId = userId,
                    UserName = fullName,
                    Balance = netBalance
                });
            }

            // 6. Run the Greedy Simplification Algorithm
            var simplifiedDebts = new List<SimplifiedDebtDto>();

            // Tolerance to avoid floating point precision issues (e.g. 0.0001)
            const decimal tolerance = 0.01m;

            while (true)
            {
                // Find the biggest debtor (most negative balance) and biggest creditor (most positive balance)
                var debtor = memberBalances.OrderBy(m => m.Balance).FirstOrDefault();
                var creditor = memberBalances.OrderByDescending(m => m.Balance).FirstOrDefault();

                if (debtor == null || creditor == null) break;

                // If balance is within tolerance of 0, we are done
                if (Math.Abs(debtor.Balance) <= tolerance || Math.Abs(creditor.Balance) <= tolerance)
                {
                    break;
                }

                // Determine the transaction amount
                decimal amountToTransfer = Math.Min(-debtor.Balance, creditor.Balance);

                if (amountToTransfer <= tolerance) break;

                simplifiedDebts.Add(new SimplifiedDebtDto
                {
                    FromUserId = debtor.UserId,
                    FromUserName = debtor.UserName,
                    ToUserId = creditor.UserId,
                    ToUserName = creditor.UserName,
                    Amount = Math.Round(amountToTransfer, 2)
                });

                // Update balances
                debtor.Balance += amountToTransfer;
                creditor.Balance -= amountToTransfer;
            }

            return new Response<List<SimplifiedDebtDto>>(simplifiedDebts);
        }

        private class MemberBalance
        {
            public string UserId { get; set; } = string.Empty;
            public string UserName { get; set; } = string.Empty;
            public decimal Balance { get; set; }
        }
    }
}
