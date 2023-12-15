using Dapper;
using Gustavo.CustomersTestAPI.Data;

namespace Gustavo.CustomersTestAPI.Repositories
{
    public class AdressesRepository : IAdressesRepository
    {
        private DbSession _dbSession;
        public AdressesRepository(DbSession dbSession)
        {
            _dbSession = dbSession;
        }

        public async Task<List<CustomerAddress>> GetAllAsync()
        {
            using (var conn = _dbSession.connection)
            {
                string query = "SELECT [AddressId], [Address], [HouseNumber], [City], [State], [Country], [PostalCode], [CustomerId] FROM [TesteAPI].[dbo].[Adresses]";
                List<CustomerAddress> adresses = (await conn.QueryAsync<CustomerAddress>(sql: query)).ToList();
                return adresses;
            }
        }

        public async Task<CustomerAddress?> GetByIdAsync(int id)
        {
            using (var conn = _dbSession.connection)
            {
                string query = "SELECT [AddressId], [Address], [HouseNumber], [City], [State], [Country], [PostalCode], [CustomerId] FROM [TesteAPI].[dbo].[Adresses] WHERE Id = @id";
                var address = await conn.QueryFirstOrDefaultAsync<CustomerAddress>
                    (sql: query, param: new { id });
                return address;
            }
        }

        public async Task<int> SaveAsync(CustomerAddress address)
        {
            using (var conn = _dbSession.connection)
            {
                string query = @"INSERT INTO Adresses([Address], [HouseNumber], [City], [State], [Country], [PostalCode], [CustomerId])
    		VALUES(@Address, @HouseNumber, @City, @State, @Country, @PostalCode, @CustomerId)";
                var result = await conn.ExecuteAsync(sql: query, param: address);
                return result;
            }
        }

        public async Task<int> UpdateAsync(CustomerAddress address)
        {
            using (var conn = _dbSession.connection)
            {
                string query = @"
    		     UPDATE Adresses SET [Address] = @Address, [HouseNumber] = @HouseNumber, [City] = @City, [State] = @State, [Country] = @Country, [PostalCode] = @PostalCode, [CustomerId] = @CustomerId WHERE AddressId = @Id";
                var result = await conn.ExecuteAsync(sql: query, param: address);
                return result;
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            using (var conn = _dbSession.connection)
            {
                string query = @"DELETE FROM Adresses WHERE AddressId = @id";
                var result = await conn.ExecuteAsync(sql: query, param: new { id });
                return result;
            }
        }

        public async Task<int> DeleteAsync(CustomerAddress address)
        {
            using (var conn = _dbSession.connection)
            {
                string query = @"DELETE FROM Adresses WHERE AddressId = @id";
                var result = await conn.ExecuteAsync(sql: query, param: new { address.AddressId });
                return result;
            }
        }
    }
}
