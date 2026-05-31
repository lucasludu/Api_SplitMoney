namespace Application.Features._groups.DTOs
{
    public class SimplifiedDebtDto
    {
        public string FromUserId { get; set; } = string.Empty;
        public string FromUserName { get; set; } = string.Empty;
        public string ToUserId { get; set; } = string.Empty;
        public string ToUserName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}
