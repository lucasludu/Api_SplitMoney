namespace Application.Features._expenses.Queries.GetExpenseAudit
{
    public class ExpenseAuditResponse
    {
        public Guid ExpenseId { get; set; }
        public List<ExpenseAuditLogEntry> History { get; set; } = new List<ExpenseAuditLogEntry>();
    }

    public class ExpenseAuditLogEntry
    {
        public string Action { get; set; } = string.Empty;
        public string? PreviousValue { get; set; }
        public string? NewValue { get; set; }
        public string ModifiedBy { get; set; } = string.Empty;
        public DateTime ChangeDate { get; set; }
    }
}
