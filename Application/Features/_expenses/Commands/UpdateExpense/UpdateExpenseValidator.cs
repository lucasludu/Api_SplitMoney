using Application.Features._expenses.DTOs;
using FluentValidation;

namespace Application.Features.Expenses.Commands.UpdateExpense
{
    public class UpdateExpenseValidator : AbstractValidator<UpdateExpenseCommand>
    {
        public UpdateExpenseValidator()
        {
            RuleFor(v => v.Request.Id)
                .NotEmpty().WithMessage("El ID del gasto es requerido.");

            RuleFor(v => v.Request.Title)
                .NotEmpty().WithMessage("El título es requerido.")
                .MaximumLength(100).WithMessage("El título no debe exceder los 100 caracteres.");

            RuleFor(v => v.Request.TotalAmount)
                .GreaterThan(0).WithMessage("El monto total debe ser mayor a 0.");

            RuleFor(v => v.Request.Currency)
                .NotEmpty().WithMessage("La moneda es requerida.")
                .Length(3).WithMessage("La moneda debe tener 3 caracteres (ej: USD, ARS).");

            RuleFor(v => v.Request.Date)
                .NotEmpty().WithMessage("La fecha es requerida.");
        }
    }
}
