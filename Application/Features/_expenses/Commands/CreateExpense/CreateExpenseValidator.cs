using Domain.Enum;
using FluentValidation;

namespace Application.Features.Expenses.Commands.CreateExpense
{
    public class CreateExpenseValidator : AbstractValidator<CreateExpenseCommand>
    {
        public CreateExpenseValidator()
        {
            RuleFor(v => v.Request.GroupId)
                .NotEmpty().WithMessage("{PropertyName} es requerido.");

            RuleFor(v => v.Request.Title)
                .NotEmpty().WithMessage("{PropertyName} no puede estar vacío.")
                .MaximumLength(150).WithMessage("{PropertyName} no debe exceder los 150 caracteres.");

            RuleFor(v => v.Request.TotalAmount)
                .GreaterThan(0).WithMessage("{PropertyName} debe ser mayor que 0.");

            RuleFor(v => v.Request.Currency)
                .NotEmpty().WithMessage("{PropertyName} es requerido.")
                .Length(3).WithMessage("{PropertyName} debe tener exactamente 3 caracteres (Ej: USD).");

            RuleFor(v => v.Request.PayerId)
                .NotEmpty().WithMessage("{PropertyName} (quién pagó) es requerido.");

            RuleFor(v => v.Request.Splits)
                .NotEmpty().WithMessage("Debe existir al menos una división del gasto.")
                .Must(x => x.Count > 0).WithMessage("Debe incluir al menos a un usuario en la división.");

            // Lógica compleja de validación según el tipo de división
            RuleFor(v => v)
                .Must(HaveValidSplitsSum)
                .WithMessage("La suma de las partes no coincide con el total del gasto.");
        }

        private bool HaveValidSplitsSum(CreateExpenseCommand command)
        {
            if (command.Request.Splits == null || !command.Request.Splits.Any()) return false;

            // Si es división por montos exactos, la suma debe dar el TotalAmount
            if (command.Request.Splits.All(s => s.SplitType == SplitTypeEnum.Exact))
            {
                var sum = command.Request.Splits.Sum(s => s.SplitValue);
                // Usamos una pequeña tolerancia para errores de redondeo decimal
                return System.Math.Abs(command.Request.TotalAmount - sum) < 0.01m;
            }

            // Si es porcentaje, la suma debe ser (aproximadamente) 100%
            if (command.Request.Splits.All(s => s.SplitType == SplitTypeEnum.Percentage))
            {
                var sum = command.Request.Splits.Sum(s => s.SplitValue);
                return System.Math.Abs(100m - sum) < 0.01m;
            }

            // Para equitativo (Equal), no validamos suma ya que el handler lo divide automáticamente
            return true;
        }
    }
}
