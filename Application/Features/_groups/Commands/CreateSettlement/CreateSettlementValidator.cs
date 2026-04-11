using FluentValidation;

namespace Application.Features.Groups.Commands
{
    public class CreateSettlementValidator : AbstractValidator<CreateSettlementCommand>
    {
        public CreateSettlementValidator()
        {
            RuleFor(v => v.Request.GroupId)
                .NotEmpty().WithMessage("El ID del grupo es requerido.");

            RuleFor(v => v.Request.PayeeId)
                .NotEmpty().WithMessage("El ID del beneficiario es requerido.");

            RuleFor(v => v.Request.Amount)
                .GreaterThan(0).WithMessage("El monto debe ser mayor a 0.");

            RuleFor(v => v.Request.Currency)
                .NotEmpty().WithMessage("La moneda es requerida.")
                .Length(3).WithMessage("La moneda debe tener 3 caracteres.");

            RuleFor(v => v.Request.Date)
                .NotEmpty().WithMessage("La fecha es requerida.");
        }
    }
}
