using Application.Wrappers;
using MediatR;

namespace Application.Features.Groups.Commands
{
    public class CreateGroupCommand : IRequest<Response<Guid>>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string DefaultCurrency { get; set; } = "USD";
        public List<MemberSpendRecord> InitialMembers { get; set; } = new();
    }

    public class MemberSpendRecord
    {
        public string Email { get; set; } = string.Empty;
        public string? FullName { get; set; }
        public decimal AmountSpent { get; set; }
    }
}
