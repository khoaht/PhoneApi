using Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class CustomerCoordinateService : ICustomerCoordinateService
    {
        private readonly IRepository<CustomerCoordinator> customerCoordinateService;
        public CustomerCoordinateService(IRepository<CustomerCoordinator> customerCoordinateService)
        {
            this.customerCoordinateService = customerCoordinateService;
        }

        public string GetExtension(int customerId,string coordinatorName)
        {
            string extension = string.Empty;

            var cust = customerCoordinateService.Get.Where(c => c.CustomerId.Equals(customerId) && c.Coordinator1.Equals(coordinatorName)).FirstOrDefault();
            if (cust != null)
            {
                extension = cust.Extension;
            }
            return extension;
        }
    }
}
