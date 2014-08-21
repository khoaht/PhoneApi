using Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> userService;
        public UserService(IRepository<User> userService)
        {
            this.userService = userService;
        }
        public bool ValidateUser(string username, string password)
        {
            //TODO: ask customer laster
            return userService.Get.Where(c => c.Login.Equals(username) && c.Password.Equals(password)).FirstOrDefault() != null; 
        }
    }
}
