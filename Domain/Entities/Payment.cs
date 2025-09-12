namespace Dapper_StoredProcedures.Domain.Entities
{
    public class Payment
    {
        public int OrderId { get; set; }
        public string Method { get; set; } = null!;
        public DateTime PaymentDate { get; set; }

        public string Status { get; set; } = "Pending";
    }
}
