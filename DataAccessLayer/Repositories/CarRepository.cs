using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppContext;
using Domains.Entities;
using Domains.Repostiories;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories;

public class CarRepository : ICarRepository
{
    private readonly DatabaseContext _context;
    private readonly DbSet<Car> _cars;

    public CarRepository(DatabaseContext context)
    {
        _context = context;
        _cars = _context.Set<Car>();
    }

    public async Task<bool> AddNewCar(Car car)
    {
        try
        {
            await _cars.AddAsync(car);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public Task<bool> DeleteCarById(int id)
    {
        try
        {
            var car = _cars.Find(id);
            _cars.Remove(car);
            _context.SaveChanges();
            return Task.FromResult(true);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }

    public Task<IEnumerable<Car>> GetAllCar()
    {
        var result = _cars
            .Include(car => car.CarBranch)
            .Include(car => car.CarStatus)
            .Include(car => car.CarSample)
            .ToList()
            .AsEnumerable();
        return Task.FromResult(result);
    }

    public Task<IEnumerable<Car>> GetCarByBranchId(int id)
    {
        var result = _cars.Where(car => car.CarBranchId == id).ToList().AsEnumerable();
        return Task.FromResult(result);
    }

    public Task<Car> GetCarById(int id)
    {
        return Task.FromResult(_cars.Find(id));
    }

    public Task<Car> GetLastedCar()
    {
        var latestCar = _cars.OrderByDescending(car => car.Id).FirstOrDefault();
        return Task.FromResult(latestCar);
    }

    public Task<bool> UpdateCarById(int id, Car car)
    {
        try
        {
            var existingCar = _cars.Find(id);
            if (existingCar == null)
            {
                return Task.FromResult(false);
            }
            existingCar.CarName = car.CarName;
            existingCar.CarPrice = car.CarPrice;
            existingCar.CarStatusId = car.CarStatusId;
            existingCar.CarBranchId = car.CarBranchId;
            existingCar.CarImage = car.CarImage;
            existingCar.CarSampleId = car.CarSampleId;
            existingCar.CarDescription = car.CarDescription;
            existingCar.CarCode = car.CarCode;
            existingCar.Color = car.Color ?? "";
            existingCar.StorageAmount = car.StorageAmount;
            _context.SaveChanges();
            return Task.FromResult(true);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }
}
