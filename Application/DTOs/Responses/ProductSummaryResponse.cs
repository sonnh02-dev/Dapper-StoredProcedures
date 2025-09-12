namespace Dapper_StoredProcedures.Application.DTOs.Responses
{
    public class ProductSummaryResponse
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public string SoldDisplay { get; set; } = null!;  // ví dụ "1 / 10"
        public string StockStatus { get; set; } = null!;
        public decimal AvgSoldPerMonth { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
    }
}
