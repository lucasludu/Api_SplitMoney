using System.Collections.Generic;

namespace Models.Response._groups
{
    public class GroupSpendingBreakdownResponse
    {
        public string GroupId { get; set; } = string.Empty;
        public string GroupName { get; set; } = string.Empty;
        public decimal TotalGroupExpense { get; set; }
        public List<MemberSpendingResponse> Members { get; set; } = new();
    }

    public class MemberSpendingResponse
    {
        public string UserId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public int TransactionCount { get; set; }
        public decimal NetBalance { get; set; } // + if owed to them, - if they owe
    }
}
