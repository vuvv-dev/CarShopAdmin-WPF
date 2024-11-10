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

public class CarStatusRepository : ICarStatusRepository
{
    private readonly DatabaseContext _context;
    private DbSet<CarStatus> _status;

    public CarStatusRepository(DatabaseContext context)
    {
        _context = context;
        _status = _context.Set<CarStatus>();
    }

    public List<CarStatus> GetAllCarStatuses()
    {
        return _status.ToList();
    }
}
