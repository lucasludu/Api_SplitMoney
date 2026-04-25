using Application.Wrappers;
using MediatR;

namespace Application.Features.Groups.Commands
{
    public class DeleteGroupCommand : IRequest<Response<Guid>>
    {
        public Guid Id { get; set; }

        public DeleteGroupCommand(Guid id)
        {
            Id = id;
        }
    }
}
