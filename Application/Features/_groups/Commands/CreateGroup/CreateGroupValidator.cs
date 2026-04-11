using Application.Features.Groups.Commands;
using FluentValidation;

namespace Application.Features.Groups.Commands.CreateGroup
{
    public class CreateGroupValidator : AbstractValidator<CreateGroupCommand>
    {
        public CreateGroupValidator()
        {
            RuleFor(v => v.Name)
                .NotEmpty().WithMessage("El nombre del grupo es requerido.")
                .MaximumLength(50).WithMessage("El nombre no debe exceder los 50 caracteres.");

            RuleFor(v => v.DefaultCurrency)
                .NotEmpty().WithMessage("La moneda por defecto es requerida.")
                .Length(3).WithMessage("La moneda debe tener 3 caracteres.");

            RuleForEach(v => v.InitialMembers).ChildRules(member =>
            {
                member.RuleFor(m => m.Email)
                    .NotEmpty().WithMessage("El email del miembro es requerido.")
                    .EmailAddress().WithMessage("El email del miembro no es válido.");
            });
        }
    }
}
