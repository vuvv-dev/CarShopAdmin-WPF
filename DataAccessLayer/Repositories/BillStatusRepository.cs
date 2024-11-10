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

public class BillStatusRepository : IBillStatusRepository
{
    private readonly DatabaseContext _context;
    private readonly DbSet<BillStatus> _status;

    public BillStatusRepository(DatabaseContext context)
    {
        _context = context;
        _status = _context.Set<BillStatus>();
    }

    public Task<IEnumerable<BillStatus>> GetAllBillStatuses()
    {
        var result = _status.ToList().AsEnumerable();
        return Task.FromResult(result);
    }
}
