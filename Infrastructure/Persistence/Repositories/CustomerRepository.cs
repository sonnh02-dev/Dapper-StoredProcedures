using Dapper;
using Dapper_StoredProcedures.Application.DTOs.Requests;
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

        public async Task<int> InsertCustomer(CreateCustomerRequest request)
        {
            var sql = @"INSERT INTO Customers (Name, Email) 
                        VALUES (@Name, @Email); 
                        SELECT CAST(SCOPE_IDENTITY() AS int)";

            using var conn = _dbFactory.OpenConnection();
            return await conn.ExecuteScalarAsync<int>(sql, request);
        }
        public async Task<int> UpdateCustomer(Customer newData)
        {
            using var conn = _dbFactory.OpenConnection();
            var sql = "UPDATE Customers SET Name = @Name, Email =@Email WHERE Id = @Id";
            return await conn.ExecuteAsync(sql, newData);
        }

        public async Task<int> DeleteCustomer(int id)
        {
            using var conn = _dbFactory.OpenConnection();
            var sql = "DELETE FROM Customers WHERE Id = @Id";
            return await conn.ExecuteAsync(sql, new { Id = id });
        }
        public async Task<bool> IsCustomerExists(int? id = null, string? email = null)
        {
            if (id == null && email == null)
                throw new ArgumentException("Phải truyền ít nhất Id hoặc Email");

            var sql = @"SELECT CASE WHEN EXISTS (
                    SELECT 1 FROM Customers
                    WHERE (@Id IS NULL OR Id = @Id)
                      AND (@Email IS NULL OR Email = @Email)
                ) THEN 1 ELSE 0 END";

            using var conn = _dbFactory.OpenConnection();
            return await conn.ExecuteScalarAsync<bool>(sql, new { Id = id, Email = email });
        }


        public async Task<IEnumerable<CustomerStatsResponse>> GetCustomersStats()
        {
            using var conn = _dbFactory.OpenConnection();
            return await conn.QueryAsync<CustomerStatsResponse>(
                "sp_GetCustomersStats",
                commandType: System.Data.CommandType.StoredProcedure
            );
        }
    }
}
