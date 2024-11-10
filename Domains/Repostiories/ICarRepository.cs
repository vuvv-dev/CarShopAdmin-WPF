using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domains.Entities;

namespace Domains.Repostiories;

public interface ICarRepository
{
    Task<bool> AddNewCar(Car car);
    Task<IEnumerable<Car>> GetAllCar();
    Task<Car> GetCarById(int id);
    Task<bool> UpdateCarById(int id, Car car);
    Task<bool> DeleteCarById(int id);

    Task<Car> GetLastedCar();

    Task<IEnumerable<Car>> GetCarByBranchId(int id);
}
