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

public class CarSampleRepository : ICarSampleRepository
{
    private readonly DatabaseContext _context;
    private readonly DbSet<CarSample> _carSample;

    public CarSampleRepository(DatabaseContext context)
    {
        _context = context;
        _carSample = _context.Set<CarSample>();
    }

    public List<CarSample> GetAllCarSamples()
    {
        return _carSample.ToList();
    }
}
