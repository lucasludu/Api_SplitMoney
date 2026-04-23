using Application.Features.Expenses.Commands.CreateExpense;
using Application.Interfaces;
using Application.Wrappers;
using Domain.Common;
using Domain.Entities;
using Domain.Enum;
using MediatR;

public class CreateExpenseCommandHandler : IRequestHandler<CreateExpenseCommand, Response<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthenticatedUserService _authenticatedUser;

    public CreateExpenseCommandHandler(IUnitOfWork unitOfWork, IAuthenticatedUserService authenticatedUser)
    {
        _unitOfWork = unitOfWork;
        _authenticatedUser = authenticatedUser;
    }

    public async Task<Response<Guid>> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = new Expense
        {
            GroupId = request.Request.GroupId,
            Title = request.Request.Title,
            Amount = new Money(request.Request.TotalAmount, request.Request.Currency),
            Date = request.Request.Date,
            CategoryId = request.Request.CategoryId,
            IsConfirmed = true
        };

        ProcessPayments(expense, request);
        ProcessSplits(expense, request);

        // Identificar si hay otros usuarios involucrados que deban confirmar
        var participants = expense.Splits.Select(s => s.UserId)
            .Union(expense.Payments.Select(p => p.UserId))
            .Where(id => id != _authenticatedUser.UserId)
            .Distinct()
            .ToList();

        if (participants.Any())
        {
            expense.IsConfirmed = false;
        }

        var newExpense = await _unitOfWork.RepositoryAsync<Expense>().AddAsync(expense);
        
        // --- Registro de Auditoría Inicial ---
        var audit = new ExpenseAudit
        {
            ExpenseId = newExpense.Id,
            Action = "Gasto Creado",
            NewValue = $"{request.Request.Title} - {request.Request.TotalAmount:C2}",
            ModifiedByUserId = _authenticatedUser.UserId,
            ChangeDate = DateTime.UtcNow
        };
        await _unitOfWork.RepositoryAsync<ExpenseAudit>().AddAsync(audit);

        // --- Crear Notificaciones ---
        foreach (var userId in participants)
        {
            var notification = new Notification
            {
                UserId = userId,
                Message = $"{_authenticatedUser.UserName ?? "Un usuario"} ha compartido un gasto contigo: {request.Request.Title}",
                Type = NotificationTypeEnum.ExpenseConfirmation,
                RelatedId = newExpense.Id,
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };
            await _unitOfWork.RepositoryAsync<Notification>().AddAsync(notification);
        }
        
        await _unitOfWork.SaveChangesAsync();

        return new Response<Guid>(newExpense.Id, expense.IsConfirmed ? "Gasto creado correctamente." : "Gasto creado y pendiente de confirmación.");
    }

    // --- MÉTODOS PRIVADOS ---

    private void ProcessPayments(Expense expense, CreateExpenseCommand request)
    {
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
    }

    private void ProcessSplits(Expense expense, CreateExpenseCommand request)
    {
        foreach (var splitDto in request.Request.Splits)
        {
            decimal amountOwed = CalculateAmountOwed(splitDto, request.Request.TotalAmount, request.Request.Splits.Count);

            expense.Splits.Add(new ExpenseSplit
            {
                UserId = splitDto.UserId,
                SplitType = splitDto.SplitType,
                SplitValue = splitDto.SplitValue,
                AmountOwed = amountOwed
            });
        }
    }

    private decimal CalculateAmountOwed(dynamic splitDto, decimal totalAmount, int totalSplits)
    {
        return splitDto.SplitType switch
        {
            SplitTypeEnum.Equal => totalAmount / totalSplits,
            SplitTypeEnum.Percentage => totalAmount * (splitDto.SplitValue / 100m),
            _ => splitDto.SplitValue // Default para monto fijo
        };
    }
}