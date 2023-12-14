using Gustavo.CustomersTestAPI.Data;

namespace Gustavo.CustomersTestAPI.Repositories
{
    public interface ICustomerRepository
    {
        Task<List<Customer>> GetAllAsync();
        Task<Customer?> GetByIdAsync(int id);
        Task<CustomerContainer> GetContainerAsync();
        Task<int> SaveAsync(Customer customer);
        Task<int> UpdateAsync(Customer customer);
        Task<int> DeleteAsync(int id);
    }
}
