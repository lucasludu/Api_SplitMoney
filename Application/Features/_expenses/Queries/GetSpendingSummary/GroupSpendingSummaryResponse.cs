namespace Application.Features.Expenses.Queries.GetSpendingSummary
{
    public class GroupSpendingSummaryResponse
    {
        public Guid GroupId { get; set; }
        public decimal TotalGroupSpending { get; set; }
        public List<CategorySpendingResponse> Categories { get; set; } = new();
    }

    public class CategorySpendingResponse
    {
        public string CategoryName { get; set; } = string.Empty;
        public string CategoryIcon { get; set; } = string.Empty;
        public string CategoryColor { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public double Percentage { get; set; }
    }
}
