using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domains.Entities;

namespace Domains.Repostiories;

public interface IBillRepository
{
    Task<bool> CreateNewBill(Bill bill);
    Task<bool> UpdateBillById(int id, Bill bill);
    Task<bool> DeleteBillById(int id);
    Task<Bill> GetBillById(int id);
    Task<IEnumerable<Bill>> GetAllBills();
    Task<IEnumerable<Bill>> SearchBills(string keyword);
}
