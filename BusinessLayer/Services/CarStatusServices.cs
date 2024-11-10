using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Repositories;
using Domains.Entities;
using Domains.Repostiories;

namespace BusinessLayer.Services;

public class CarStatusServices
{
    private readonly ICarStatusRepository _carStatusRepository;

    public CarStatusServices()
    {
        _carStatusRepository = new CarStatusRepository(new AppContext.DatabaseContext());
    }

    public List<CarStatus> GetAllCarStatuses()
    {
        return _carStatusRepository.GetAllCarStatuses();
    }
}
