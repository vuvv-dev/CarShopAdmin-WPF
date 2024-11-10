using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Repositories;
using Domains.Entities;
using Domains.Repostiories;

namespace BusinessLayer.Services;

public class CarSampleServices
{
    private readonly ICarSampleRepository _carSampleRepository;

    public CarSampleServices()
    {
        _carSampleRepository = new CarSampleRepository(new AppContext.DatabaseContext());
    }

    public List<CarSample> GetAllCarSamples() => _carSampleRepository.GetAllCarSamples();
}
