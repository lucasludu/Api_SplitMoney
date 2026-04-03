using Application.Wrappers;
using MediatR;

namespace Application.Features._groups.Commands.CreateGroup
{
    public class CreateGroupCommand : IRequest<Response<Guid>>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string DefaultCurrency { get; set; } = "USD";
        public List<string> MemberIds { get; set; } = new List<string>();
    }
}
