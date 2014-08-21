using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Domain;
namespace Infrastructure
{
    public interface ICustomerService 
    {
        bool ValidateUser(string username,string password);

        string GetExtension(string coordinatorName);

    }
}
