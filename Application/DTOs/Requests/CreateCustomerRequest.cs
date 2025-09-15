namespace Dapper_StoredProcedures.Application.DTOs.Requests
{
    public class CreateCustomerRequest
    {
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
    }
}
