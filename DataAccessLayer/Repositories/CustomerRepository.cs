using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppContext;
using Domains.Entities;
using Domains.Repostiories;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly DatabaseContext _context;
    private readonly DbSet<Customer> _customer;

    public CustomerRepository(DatabaseContext context)
    {
        _context = context;
        _customer = _context.Set<Customer>();
    }

    public Task<bool> AddNewCustomer(Customer customer)
    {
        try
        {
            _customer.Add(customer);
            _context.SaveChanges();
            return Task.FromResult(true);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }

    public Task<bool> DeleteCustomerById(int id)
    {
        try
        {
            var customer = _customer.Find(id);
            _customer.Remove(customer);
            _context.SaveChanges();
            return Task.FromResult(true);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }

    public Task<IEnumerable<Customer>> GetAllCustomers()
    {
        var result = _customer.ToList().AsEnumerable();
        return Task.FromResult(result);
    }

    public Task<Customer> GetCustomerById(int id)
    {
        try
        {
            var result = _customer.Find(id);
            return Task.FromResult(result!);
        }
        catch
        {
            return Task.FromResult<Customer>(null);
        }
    }

    public Task<IEnumerable<Customer>> SearchCustomerByName(string name)
    {
        var result = _customer
            .Where(customer => customer.Name.Contains(name))
            .ToList()
            .AsEnumerable();

        return Task.FromResult(result);
    }

    public Task<bool> UpdateCustomerById(int id, Customer customer)
    {
        try
        {
            var foundCustomer = _customer.Find(id);

            if (foundCustomer != null)
            {
                foundCustomer!.Address = customer.Address;
                foundCustomer.Phone = customer.Phone;
                foundCustomer.Email = customer.Email;
                foundCustomer.Name = customer.Name;

                _context.SaveChanges();
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }
}
