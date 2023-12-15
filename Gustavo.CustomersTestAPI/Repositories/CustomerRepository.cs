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

                var query = @"
                SELECT 
                    c.CustomerId,
                    c.ClientType,
                    c.CPF,
                    c.CNPJ,
                    c.FullName,
                    c.CompanyName,
                    c.TradeName,
                    a.AddressId,
                    a.Address,
                    a.HouseNumber,
                    a.City,
                    a.State,
                    a.Country,
                    a.PostalCode,
                    a.CustomerId
                FROM Customers c
                LEFT JOIN Adresses a ON c.CustomerId = a.CustomerId";

                var customersList = new List<Customer>();
                var result = await conn.QueryAsync<Customer, CustomerAddress, Customer>(
                    query,
                    (customer, address) =>
                    {
                        var CustomerCached = customersList.FirstOrDefault(c => c.CustomerId == customer.CustomerId);

                        if (CustomerCached == null)
                        {
                            customer.Adresses = new List<CustomerAddress> { address };
                            customersList.Add(customer);
                        }
                        else
                        {
                            CustomerCached.Adresses.Add(address);
                        }

                        return null;
                    },
                    splitOn: "AddressId");

                return customersList;

                /*
                string query = "SELECT [Id], [ClientType], [CPF], [CNPJ], [FullName], [CompanyName], [TradeName] FROM [TesteAPI].[dbo].[Customers]";
                List<Customer> customers = (await conn.QueryAsync<Customer>(sql: query)).ToList();
                return customers;
                */
            }
        }

        public async Task<Customer?> GetByIdWithAdressesAsync(int CustomerId)
        {
            using (var conn = _dbSession.connection)
            {
                var query = @"
                SELECT 
                    c.CustomerId,
                    c.ClientType,
                    c.CPF,
                    c.CNPJ,
                    c.FullName,
                    c.CompanyName,
                    c.TradeName,
                    a.AddressId,
                    a.Address,
                    a.HouseNumber,
                    a.City,
                    a.State,
                    a.Country,
                    a.PostalCode,
                    a.CustomerId
                FROM Customers c
                LEFT JOIN Adresses a ON c.CustomerId = a.CustomerId
                WHERE c.CustomerId = @CustomerId";

                /*

                var customersDictionary = new Dictionary<int, Customer>();
                var result = conn.Query<Customer, CustomerAddress, Customer>(
                    query,
                    (customer, address) =>
                    {
                        if (!customersDictionary.TryGetValue(customer.Id, out var customerEntry))
                        {
                            customerEntry = customer;
                            customerEntry.Adresses = new List<CustomerAddress>();
                            customersDictionary.Add(customerEntry.Id, customerEntry);
                        }

                        if (address != null)
                        {
                            customerEntry.Adresses.Add(address);
                        }

                        return customerEntry;
                    },
                new { CustomerId },
                splitOn: "AddressId")
                .Distinct()
                .SingleOrDefault();

                return result;

                */

                var customersList = new List<Customer>();
                var result = await conn.QueryAsync<Customer, CustomerAddress, Customer>(
                    query,
                    (customer, address) =>
                    {
                        var CustomerCached = customersList.FirstOrDefault(c => c.CustomerId == customer.CustomerId);

                        if (CustomerCached == null)
                        {
                            customer.Adresses = new List<CustomerAddress> { address };
                            customersList.Add(customer);
                        }
                        else
                        {
                            CustomerCached.Adresses.Add(address);
                        }

                        return null;
                    },
                    new { CustomerId },
                    splitOn: "AddressId");

                return customersList.First();
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
    		     UPDATE Customers SET [ClientType] = @ClientType, [CPF] = @CPF, [CNPJ] = @CNPJ, [FullName] = @FullName, [CompanyName] = @CompanyName, [TradeName] = @TradeName WHERE Id = @Id";
                var result = await conn.ExecuteAsync(sql: query, param: customer);
                return result;
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            using (var conn = _dbSession.connection)
            {
                string query = @"DELETE FROM Customers WHERE CustomerId = @id";
                var result = await conn.ExecuteAsync(sql: query, param: new { id });
                return result;
            }
        }

        public async Task<int> DeleteAsync(Customer customer)
        {
            using (var conn = _dbSession.connection)
            {
                string query = @"DELETE FROM Customers WHERE CustomerId = @id";
                var result = await conn.ExecuteAsync(sql: query, param: new { customer.CustomerId });
                return result;
            }
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            using (var conn = _dbSession.connection)
            {

                string query = "SELECT [Id], [ClientType], [CPF], [CNPJ], [FullName], [CompanyName], [TradeName] FROM [TesteAPI].[dbo].[Customers] WHERE CustomerId = @id";
                var customer = await conn.QueryFirstOrDefaultAsync<Customer>
                    (sql: query, param: new { id });
                return customer;

            }
        }
    }
}
