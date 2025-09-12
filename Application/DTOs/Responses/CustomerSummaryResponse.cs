namespace Dapper_StoredProcedures.Application.DTOs.Responses
{
    public class CustomerSummaryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public int TotalOrders { get; set; }
        public int TotalPurchasedProducts { get; set; }
        public string? FavoriteProduct { get; set; }
        public int TotalFailedPayments { get; set; }
        public int TotalSuccessfulPayments { get; set; }
        public decimal TotalSpent { get; set; }
        public DateTime? LastOrderDate { get; set; }
    }
}
