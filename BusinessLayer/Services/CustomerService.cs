using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Repositories;
using Domains.Entities;
using Domains.Repostiories;

namespace BusinessLayer.Services;

public class CustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService()
    {
        _customerRepository = new CustomerRepository(new AppContext.DatabaseContext());
    }

    public Task<bool> AddNewCustomer(Customer customer)
    {
        return _customerRepository.AddNewCustomer(customer);
    }

    public Task<bool> UpdateCustomerById(int id, Customer customer)
    {
        return _customerRepository.UpdateCustomerById(id, customer);
    }

    public Task<IEnumerable<Customer>> GetAllCustomer()
    {
        return _customerRepository.GetAllCustomers();
    }

    public Task<bool> DeleteCustomerById(int id)
    {
        return _customerRepository.DeleteCustomerById(id);
    }

    public Task<Customer> GetCustomerById(int id)
    {
        return _customerRepository.GetCustomerById(id);
    }

    public Task<IEnumerable<Customer>> SearchCustomerByName(string name)
    {
        return _customerRepository.SearchCustomerByName(name);
    }
}
