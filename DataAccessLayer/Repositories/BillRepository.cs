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

public class BillRepository : IBillRepository
{
    private readonly DatabaseContext _context;
    private readonly DbSet<Bill> _bills;

    public BillRepository(DatabaseContext context)
    {
        _context = context;
        _bills = _context.Set<Bill>();
    }

    public Task<bool> CreateNewBill(Bill bill)
    {
        try
        {
            _bills.Add(bill);
            _context.SaveChanges();
            return Task.FromResult(true);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }

    public Task<bool> DeleteBillById(int id)
    {
        try
        {
            var bill = _bills.Find(id);
            _bills.Remove(bill);
            _context.SaveChanges();
            return Task.FromResult(true);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }

    public Task<IEnumerable<Bill>> GetAllBills()
    {
        try
        {
            var result = _bills
                .Include(bill => bill.Customer)
                .Include(bill => bill.BillStatus)
                .Include(bill => bill.Car)
                .ToList()
                .AsEnumerable();
            return Task.FromResult(result);
        }
        catch
        {
            return Task.FromResult<IEnumerable<Bill>>(null);
        }
    }

    public Task<Bill> GetBillById(int id)
    {
        try
        {
            var result = _bills
                .Include(bill => bill.Customer)
                .Include(bill => bill.BillStatus)
                .Include(bill => bill.Car)
                .Where(bill => bill.Id == id)
                .FirstOrDefault();
            return Task.FromResult(result);
        }
        catch
        {
            return Task.FromResult<Bill>(null);
        }
    }

    public Task<IEnumerable<Bill>> SearchBills(string keyword)
    {
        try
        {
            var result = _bills
                .Include(bill => bill.Customer)
                .Include(bill => bill.BillStatus)
                .Include(bill => bill.Car)
                .Where(bill =>
                    bill.Customer.Name.Contains(keyword)
                    || bill.BillStatus.BillStatusName.Contains(keyword)
                    || bill.Car.CarName.Contains(keyword)
                )
                .ToList()
                .AsEnumerable();
            return Task.FromResult(result);
        }
        catch
        {
            return Task.FromResult<IEnumerable<Bill>>(null);
        }
    }

    public Task<bool> UpdateBillById(int id, Bill bill)
    {
        try
        {
            var existingBill = _bills.Find(id);
            if (existingBill == null)
            {
                return Task.FromResult(false);
            }
            existingBill.CustomerId = bill.CustomerId;
            existingBill.BillStatusId = bill.BillStatusId;
            existingBill.CarId = bill.CarId;
            existingBill.UpdateDate = DateTime.Now;
            _context.SaveChanges();
            return Task.FromResult(true);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }
}
