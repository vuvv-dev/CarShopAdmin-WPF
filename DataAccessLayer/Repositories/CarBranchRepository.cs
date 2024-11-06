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

public class CarBranchRepository : ICarBranchesRepository
{
    private readonly DatabaseContext _context;
    private readonly DbSet<CarBranch> _carBranch;

    public CarBranchRepository(DatabaseContext context)
    {
        _context = context;
        _carBranch = _context.Set<CarBranch>();
    }

    public Task<CarBranch> AddCarBranches(CarBranch carBranches)
    {
        try
        {
            _carBranch.Add(carBranches);
            _context.SaveChanges();
            return Task.FromResult(carBranches);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public Task<bool> DeleteCarBranchesById(int id)
    {
        var carBranch = _context.CarBranches.Find(id); // Assuming _context is your database context

        if (carBranch != null)
        {
            // Remove the found branch from the context
            _context.CarBranches.Remove(carBranch);
            // Save changes to the database
            _context.SaveChanges();

            // Return true to indicate success
            return Task.FromResult(true);
        }
        else
        {
            // If branch is not found, return false
            return Task.FromResult(false);
        }
    }

    public List<CarBranch> GetAllCarBranches()
    {
        return _carBranch.ToList();
    }

    public Task<CarBranch> GetCarBranchesById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<CarBranch> UpdateCarBranches(CarBranch carBranches)
    {
        _carBranch.Update(carBranches);
        _context.SaveChanges();
        return Task.FromResult(carBranches);
    }

    public CarBranch UpdateCarBranchById(int id, CarBranch updatedCarBranch)
    {
        // Retrieve the existing CarBranch from the database by its ID
        var existingBranch = _context.CarBranches.Find(id);

        // Check if the existing branch was found
        if (existingBranch == null)
        {
            throw new KeyNotFoundException($"CarBranch with ID {id} not found.");
        }

        // Update the properties of the existing branch with the new values
        existingBranch.BranchName = updatedCarBranch.BranchName;
        existingBranch.BranchLogo = updatedCarBranch.BranchLogo;

        // Save the changes to the database
        _context.SaveChanges();

        // Return the updated CarBranch
        return existingBranch;
    }
}
