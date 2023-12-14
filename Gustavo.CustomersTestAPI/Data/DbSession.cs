using System.Data;
using System.Data.SqlClient;

namespace Gustavo.CustomersTestAPI.Data
{
    public class DbSession : IDisposable
    {
        public IDbConnection connection { get; }

        public DbSession(IConfiguration configuration)
        {
            try
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                Console.WriteLine(connectionString);
                connection = new SqlConnection(connectionString);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public void Dispose() => connection?.Dispose();
    }
}
