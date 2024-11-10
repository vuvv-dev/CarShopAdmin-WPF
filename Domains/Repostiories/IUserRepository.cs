using Domains.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domains.Repostiories;

public interface IUserRepository
{
    public Task<User> GetUserByUsername(string username);
}
