using Domain.Enum;

namespace Application.Features._expenses.DTOs
{
    public class ExpenseSplitRequest
    {
        public string UserId { get; set; } = string.Empty;
        public SplitTypeEnum SplitType { get; set; }
        public decimal SplitValue { get; set; }
    }
}
