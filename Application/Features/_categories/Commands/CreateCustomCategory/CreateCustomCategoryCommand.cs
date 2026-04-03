using Application.Wrappers;
using MediatR;

namespace Application.Features._categories.Commands.CreateCustomCategory
{
    public class CreateCustomCategoryCommand : IRequest<Response<Guid>>
    {
        public string Name { get; set; } = string.Empty;
        public string IconIdentifier { get; set; } = string.Empty;
        public string ColorHex { get; set; } = "#000000";
    }
}
