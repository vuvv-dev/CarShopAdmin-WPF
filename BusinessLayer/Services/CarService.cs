using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Repositories;
using Domains.Entities;
using Domains.Repostiories;

namespace BusinessLayer.Services;

public class CarService
{
    private readonly ICarRepository _carRepository;

    public CarService()
    {
        _carRepository = new CarRepository(new AppContext.DatabaseContext());
    }

    public async Task<bool> AddNewCar(Car car)
    {
        return await _carRepository.AddNewCar(car);
    }

    public Task<IEnumerable<Car>> GetAllCar() => _carRepository.GetAllCar();

    public Task<Car> GetCarById(int id) => _carRepository.GetCarById(id);

    public Task<Car> GetLastedCar()
    {
        return _carRepository.GetLastedCar();
    }

    public Task<bool> DeleteCarById(int id)
    {
        return _carRepository.DeleteCarById(id);
    }

    public Task<IEnumerable<Car>> GetCarByBranchId(int id)
    {
        return _carRepository.GetCarByBranchId((int)id);
    }

    public Task<bool> UpdateCarById(int id, Car car)
    {
        return _carRepository.UpdateCarById(id, car);
    }
}
