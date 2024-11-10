using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppContext;
using DataAccessLayer.Repositories;
using Domains.Entities;
using Domains.Repostiories;

namespace BusinessLayer.Services;

public class UserService
{
    private readonly IUserRepository _userRepository;

    public UserService()
    {
        _userRepository = new UserRepository(new AppContext.DatabaseContext());
    }

    public Task<User> GetUserByUsername(string username) =>
        _userRepository.GetUserByUsername(username);
}
