using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domains.Entities;

namespace Domains.Repostiories;

public interface ICarBranchesRepository
{
    List<CarBranch> GetAllCarBranches();

    Task<CarBranch> GetCarBranchesById(int id);
    Task<CarBranch> AddCarBranches(CarBranch carBranches);
    Task<CarBranch> UpdateCarBranches(CarBranch carBranches);
    Task<bool> DeleteCarBranchesById(int id);
    CarBranch UpdateCarBranchById(int id, CarBranch updatedCarBranch);
}
