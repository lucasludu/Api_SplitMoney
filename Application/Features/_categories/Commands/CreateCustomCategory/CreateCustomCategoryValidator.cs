using FluentValidation;

namespace Application.Features._categories.Commands.CreateCustomCategory
{
    public class CreateCustomCategoryValidator : AbstractValidator<CreateCustomCategoryCommand>
    {
        public CreateCustomCategoryValidator()
        {
            RuleFor(v => v.Name)
                .NotEmpty().WithMessage("El nombre de la categoría es requerido.")
                .MaximumLength(50).WithMessage("El nombre no debe exceder los 50 caracteres.");

            RuleFor(v => v.IconIdentifier)
                .NotEmpty().WithMessage("El icono es requerido.");

            RuleFor(v => v.ColorHex)
                .NotEmpty().WithMessage("El color es requerido.")
                .Matches("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$").WithMessage("El color debe ser un formato hexadecimal válido.");
        }
    }
}
