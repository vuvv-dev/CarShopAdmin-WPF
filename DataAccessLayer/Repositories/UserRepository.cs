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

public class UserRepository : IUserRepository
{
    private readonly DatabaseContext _context;
    private readonly DbSet<User> _user;

    public UserRepository(DatabaseContext context)
    {
        _context = context;
        _user = _context.Set<User>();
    }

    public Task<User> GetUserByUsername(string username)
    {
        var result = _user.Where(user => user.Username == username).FirstOrDefault();
        return Task.FromResult(result!);
    }
}
