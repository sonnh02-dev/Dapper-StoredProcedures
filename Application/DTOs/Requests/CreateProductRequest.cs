namespace Dapper_StoredProcedures.Application.DTOs.Requests
{
    public class CreateProductRequest
    {
        public string? SKU { get; set; } = null!;

        public string Name { get; set; } = default!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public int CategoryId { get; set; }
    }
}
