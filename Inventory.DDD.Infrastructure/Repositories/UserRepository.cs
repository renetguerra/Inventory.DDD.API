using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventory.DDD.Domain.Entities;
using Inventory.DDD.Domain.IRepositories;

namespace Inventory.DDD.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        List<User> users = new List<User>()
        {
            new User(){ Email = "user1@hotmail.com", Password = "123456"},
            new User(){ Email = "user2@hotmail.com", Password = "123456"}
        };

        public bool IsUser(string email, string password)
        {
            return users.Where(u => u.Email == email && u.Password == password).Count() > 0;
        }
    }
}
