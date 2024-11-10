using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Repositories;
using Domains.Entities;
using Domains.Repostiories;

namespace BusinessLayer.Services;

public class BillServices
{
    private readonly IBillRepository _billRepository;

    public BillServices()
    {
        _billRepository = new BillRepository(new AppContext.DatabaseContext());
    }

    public Task<IEnumerable<Bill>> GetAllBills() => _billRepository.GetAllBills();

    public Task<Bill> GetBillById(int id) => _billRepository.GetBillById(id);

    public Task<bool> AddNewBill(Bill bill) => _billRepository.CreateNewBill(bill);

    public Task<bool> UpdateBillById(int id, Bill bill) => _billRepository.UpdateBillById(id, bill);

    public Task<bool> DeleteBillById(int id) => _billRepository.DeleteBillById(id);

    public Task<IEnumerable<Bill>> SearchBills(string key) => _billRepository.SearchBills(key);

}
