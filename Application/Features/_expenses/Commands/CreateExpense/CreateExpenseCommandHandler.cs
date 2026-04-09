using Application.Features.Expenses.Commands.CreateExpense;
using Application.Interfaces;
using Application.Wrappers;
using Domain.Entities;
using Domain.Enum;
using MediatR;

namespace Application.Features.Expenses.Commands.CreateExpense
{
    public class CreateExpenseCommandHandler : IRequestHandler<CreateExpenseCommand, Response<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateExpenseCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<Guid>> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
        {
            var expense = new Expense
            {
                GroupId = request.Request.GroupId,
                Title = request.Request.Title,
                TotalAmount = request.Request.TotalAmount,
                Currency = request.Request.Currency,
                Date = request.Request.Date,
                CategoryId = request.Request.CategoryId,
                PayerId = request.Request.PayerId ?? request.Request.Payments.FirstOrDefault()?.UserId ?? string.Empty
            };
            
            // Process Payers (Multiple or Single)
            if (request.Request.Payments != null && request.Request.Payments.Any())
            {
                foreach (var paymentDto in request.Request.Payments)
                {
                    expense.Payments.Add(new ExpensePayment
                    {
                        UserId = paymentDto.UserId,
                        AmountPaid = paymentDto.AmountPaid
                    });
                }
            }
            else if (!string.IsNullOrEmpty(request.Request.PayerId))
            {
                expense.Payments.Add(new ExpensePayment
                {
                    UserId = request.Request.PayerId,
                    AmountPaid = request.Request.TotalAmount
                });
            }

            foreach (var splitDto in request.Request.Splits)
            {
                var amountOwed = 0m;
                if (splitDto.SplitType == SplitTypeEnum.Equal)
                {
                    amountOwed = request.Request.TotalAmount / request.Request.Splits.Count;
                }
                else if (splitDto.SplitType == SplitTypeEnum.Percentage)
                {
                    amountOwed = request.Request.TotalAmount * (splitDto.SplitValue / 100m);
                }
                else
                {
                    amountOwed = splitDto.SplitValue;
                }

                expense.Splits.Add(new ExpenseSplit
                {
                    UserId = splitDto.UserId,
                    SplitType = splitDto.SplitType,
                    SplitValue = splitDto.SplitValue,
                    AmountOwed = amountOwed
                });
            }

            var newExpense = await _unitOfWork.RepositoryAsync<Expense>().AddAsync(expense);
            await _unitOfWork.SaveChangesAsync();    

            return new Response<Guid>(newExpense.Id, "Gasto creado correctamente.");
        }
    }

}
