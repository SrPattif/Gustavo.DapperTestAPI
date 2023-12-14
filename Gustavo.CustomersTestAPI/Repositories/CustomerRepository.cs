using System.Data.SqlClient;
using Dapper;
using Gustavo.CustomersTestAPI.Data;

namespace Gustavo.CustomersTestAPI.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private DbSession _dbSession;
        public CustomerRepository(DbSession dbSession)
        {
            _dbSession = dbSession;
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            using(var conn = _dbSession.connection)
            {
                string query = "SELECT [Id], [ClientType], [CPF], [CNPJ], [FullName], [CompanyName], [TradeName] FROM [TesteAPI].[dbo].[Customers]";
                List<Customer> customers = (await conn.QueryAsync<Customer>(sql: query)).ToList();
                return customers;
            }
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            using (var conn = _dbSession.connection)
            {
                string query = "SELECT [Id], [ClientType], [CPF], [CNPJ], [FullName], [CompanyName], [TradeName] FROM [TesteAPI].[dbo].[Customers] WHERE Id = @id";
                var customer = await conn.QueryFirstOrDefaultAsync<Customer>
                    (sql: query, param: new { id });
                return customer;
            }
        }

        public async Task<CustomerContainer> GetContainerAsync()
        {
            using (var conn = _dbSession.connection)
            {
                string query =
                    @"SELECT COUNT(*) FROM Customers
    	          SELECT * FROM Customers";
                var reader = await conn.QueryMultipleAsync(sql: query);
                return new CustomerContainer
                {
                    Counter = (await reader.ReadAsync<int>()).FirstOrDefault(),
                    Customers = (await reader.ReadAsync<Customer>()).ToList()
                };
            }
        }

        public async Task<int> SaveAsync(Customer customer)
        {
            using (var conn = _dbSession.connection)
            {
                string query = @"INSERT INTO Customers([ClientType], [CPF], [CNPJ], [FullName], [CompanyName], [TradeName])
    		VALUES(@ClientType, @CPF, @CNPJ, @FullName, @CompanyName, @TradeName)";
                var result = await conn.ExecuteAsync(sql: query, param: customer);
                return result;
            }
        }

        public async Task<int> UpdateAsync(Customer customer)
        {
            using (var conn = _dbSession.connection)
            {
                string query = @"
    		     UPDATE Customers SET [ClientType] = @IsCompleta, [CPF] = @CPF, [CNPJ] = @CNPJ, [FullName] = @FullName, [CompanyName] = @CompanyName, [TradeName] = @TradeName WHERE Id = @Id";
                var result = await conn.ExecuteAsync(sql: query, param: customer);
                return result;
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            using (var conn = _dbSession.connection)
            {
                string query = @"DELETE FROM Tarefas WHERE Id = @id";
                var result = await conn.ExecuteAsync(sql: query, param: new { id });
                return result;
            }
        }

        public async Task<int> DeleteAsync(Customer customer)
        {
            using (var conn = _dbSession.connection)
            {
                string query = @"DELETE FROM Tarefas WHERE Id = @id";
                var result = await conn.ExecuteAsync(sql: query, param: new { customer.Id });
                return result;
            }
        }
    }
}
