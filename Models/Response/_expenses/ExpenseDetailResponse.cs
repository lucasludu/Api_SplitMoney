using System;
using System.Collections.Generic;

namespace Models.Response._expenses
{
    public class ExpenseDetailResponse
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public DateTime Date { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string GroupName { get; set; } = string.Empty;
        public string CategoryIcon { get; set; } = "💰";
        public string CategoryColor { get; set; } = "#000000";
        public List<PaymentDetailResponse> Payments { get; set; } = new();
        public List<SplitDetailResponse> Splits { get; set; } = new();
    }

    public class PaymentDetailResponse
    {
        public string UserName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }

    public class SplitDetailResponse
    {
        public string UserName { get; set; } = string.Empty;
        public decimal AmountOwed { get; set; }
    }
}
