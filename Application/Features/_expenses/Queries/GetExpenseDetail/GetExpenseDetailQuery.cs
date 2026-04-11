using Application.Interfaces;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Features._expenses.DTOs;

namespace Application.Features.Expenses.Queries
{
    public record GetExpenseDetailQuery(Guid Id) : IRequest<Response<ExpenseDetailResponse>>;

    public class GetExpenseDetailQueryHandler : IRequestHandler<GetExpenseDetailQuery, Response<ExpenseDetailResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetExpenseDetailQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<ExpenseDetailResponse>> Handle(GetExpenseDetailQuery request, CancellationToken cancellationToken)
        {
            var expense = await _unitOfWork.RepositoryAsync<Expense>()
                .Entities
                .Include(e => e.Group)
                .Include(e => e.Category)
                .Include(e => e.Payments)
                    .ThenInclude(p => p.User)
                .Include(e => e.Splits)
                    .ThenInclude(s => s.User)
                .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

            if (expense == null)
                return new Response<ExpenseDetailResponse>("Gasto no encontrado.");

            var details = new ExpenseDetailResponse
            {
                Id = expense.Id,
                Description = expense.Title,
                TotalAmount = expense.Amount.Amount,
                Date = expense.Created,
                GroupName = expense.Group.Name,
                CategoryIcon = expense.Category != null ? expense.Category.IconIdentifier : "💰",
                CategoryColor = expense.Category != null ? expense.Category.ColorHex : "#000000",
                Payments = expense.Payments.Select(p => new PaymentDetailResponse
                {
                    UserName = $"{p.User.FirstName} {p.User.LastName}",
                    Amount = p.AmountPaid
                }).ToList(),
                Splits = expense.Splits.Select(s => new SplitDetailResponse
                {
                    UserName = $"{s.User.FirstName} {s.User.LastName}",
                    AmountOwed = s.AmountOwed
                }).ToList()
            };

            return new Response<ExpenseDetailResponse>(details);
        }
    }
}
