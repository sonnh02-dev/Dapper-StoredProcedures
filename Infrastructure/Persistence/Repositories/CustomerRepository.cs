using Dapper;
using Dapper_StoredProcedures.Application.DTOs.Responses;
using Dapper_StoredProcedures.Domain.Entities;
using Dapper_StoredProcedures.Domain.IRepositories;
using Dapper_StoredProcedures.Infrastructure.Persistence;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dapper_StoredProcedures.Infrastructure.Persistence.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DbConnectionFactory _dbFactory;

        public CustomerRepository(DbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<int> InsertCustomer(Customer customer)
        {
            var sql = @"INSERT INTO Customers (Name, Phone) 
                        VALUES (@Name, @Phone); 
                        SELECT CAST(SCOPE_IDENTITY() AS int)";

            using var conn = _dbFactory.OpenConnection();
            return await conn.ExecuteScalarAsync<int>(sql, customer);
        }

        public async Task<bool> IsCustomerExists(string name)
        {
            var sql = @"SELECT CASE WHEN EXISTS (
                            SELECT 1 FROM Customers WHERE Name = @Name
                        ) THEN 1 ELSE 0 END";

            using var conn = _dbFactory.OpenConnection();
            return await conn.ExecuteScalarAsync<bool>(sql, new { Name = name });
        }

        public async Task<IEnumerable<CustomerSummaryResponse>> GetCustomerSummaries()
        {
            using var conn = _dbFactory.OpenConnection();
            return await conn.QueryAsync<CustomerSummaryResponse>(
                "sp_GetCustomerSummaries",
                commandType: System.Data.CommandType.StoredProcedure
            );
        }
    }
}
