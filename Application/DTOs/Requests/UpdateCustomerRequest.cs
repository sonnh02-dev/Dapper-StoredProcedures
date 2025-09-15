namespace Dapper_StoredProcedures.Application.DTOs.Requests
{
    public class UpdateCustomerRequest
    {
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;

    }
}
