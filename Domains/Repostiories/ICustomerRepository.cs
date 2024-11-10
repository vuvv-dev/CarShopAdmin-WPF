using Domains.Entities;

namespace Domains.Repostiories;

public interface ICustomerRepository
{
    Task<bool> AddNewCustomer(Customer customer);
    Task<IEnumerable<Customer>> GetAllCustomers();
    Task<Customer> GetCustomerById(int id);
    Task<bool> UpdateCustomerById(int id, Customer customer);
    Task<bool> DeleteCustomerById(int id);
    Task<IEnumerable<Customer>> SearchCustomerByName(string name);
}
