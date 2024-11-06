using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Repositories;
using Domains.Entities;
using Domains.Repostiories;

namespace BusinessLayer.Services;

public class CarBranchServices
{
    private readonly ICarBranchesRepository _carBranchesRepository;

    public CarBranchServices()
    {
        _carBranchesRepository = new CarBranchRepository(new AppContext.DatabaseContext());
    }

    public Task<CarBranch> AddCarBranches(CarBranch carBranches)
    {
        return _carBranchesRepository.AddCarBranches(carBranches);
    }

    public List<CarBranch> GetAllCarBranches()
    {
        return _carBranchesRepository.GetAllCarBranches();
    }

    public CarBranch UpdateCarBranchesById(int id, CarBranch carBranches)
    {
        return _carBranchesRepository.UpdateCarBranchById(id, carBranches);
    }

    public Task<bool> DeleteCarBranchesById(int id)
    {
        return _carBranchesRepository.DeleteCarBranchesById(id);
    }
}
