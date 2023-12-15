using Gustavo.CustomersTestAPI.Data;

namespace Gustavo.CustomersTestAPI.Repositories
{
    public interface IAdressesRepository
    {
        Task<List<CustomerAddress>> GetAllAsync();
        Task<CustomerAddress?> GetByIdAsync(int id);
        Task<int> SaveAsync(CustomerAddress address);
        Task<int> UpdateAsync(CustomerAddress address);
        Task<int> DeleteAsync(int id);
    }
}
