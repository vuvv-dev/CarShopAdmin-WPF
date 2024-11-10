using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Repositories;
using Domains.Entities;
using Domains.Repostiories;

namespace BusinessLayer.Services;

public class BillStatuesServices
{
    private readonly IBillStatusRepository _billStatusRepository;

    public BillStatuesServices()
    {
        _billStatusRepository = new BillStatusRepository(new AppContext.DatabaseContext());
    }

    public Task<IEnumerable<BillStatus>> GetAllBillStatuses() =>
        _billStatusRepository.GetAllBillStatuses();
}
