using System;
using System.Collections.Generic;

namespace Application.Features._expenses.DTOs
{
    public class DashboardResponse
    {
        public decimal TotalToReceive { get; set; }
        public decimal TotalToPay { get; set; }
        public decimal TotalMonthSpending { get; set; }
        public List<RecentExpenseResponse> RecentExpenses { get; set; } = new();
    }

    public class RecentExpenseResponse
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;
        public string GroupName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string CategoryIcon { get; set; } = "💰";
        public string CategoryColor { get; set; } = "#000000";
    }
}
