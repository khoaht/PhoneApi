using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Domain
{
    public class LookupData
    {
        public string Branch { get; set; }
        public string City { get; set; }
        public string Coordinator1 { get; set; }
        public string Coordinator2 { get; set; }
        public string Coordinator3 { get; set; }
        public string Location { get; set; }
        public string Nurse { get; set; }
        public string PrimaryContract { get; set; }
        public string PrimaryLanguage { get; set; }
        public string State { get; set; }
        public string Team { get; set; }
        public string Type { get; set; }
        public string Zip { get; set; }
        public int Extension { get; set; }
        public string Phone { get; set; }
        public List<Service> Services { get; set; }
    }

    public class Service {
        public string ServiceName { get; set; }
    }
}
