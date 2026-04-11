using FluentValidation;

namespace Application.Features.Expenses.Commands.DeleteExpense
{
    public class DeleteExpenseValidator : AbstractValidator<DeleteExpenseCommand>
    {
        public DeleteExpenseValidator()
        {
            RuleFor(v => v.Id)
                .NotEmpty().WithMessage("El ID del gasto es requerido.");
        }
    }
}
