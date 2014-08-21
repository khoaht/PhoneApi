using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Domain;
namespace Infrastructure
{
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<Customer> customerRepository;
        public CustomerService(IRepository<Customer> customerRepository)
        {
            this.customerRepository = customerRepository;
        }
        public bool ValidateUser(string username, string password)
        {
            //TODO: ask customer laster
            return true;
        }


        public string GetExtension(string coordinatorName)
        {
            throw new NotImplementedException();
        }
    }
}
