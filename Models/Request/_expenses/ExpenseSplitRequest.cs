using Domain.Enum;

namespace Models.Request._expenses
{
    public class ExpenseSplitRequest
    {
        public string UserId { get; set; } = string.Empty;
        public SplitTypeEnum SplitType { get; set; }
        public decimal SplitValue { get; set; }
    }
}
