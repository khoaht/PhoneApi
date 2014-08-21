//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Infrastructure.Domain
{
    using System;
    using System.Collections.Generic;
    
    public partial class Customer
    {
        public Customer()
        {
            this.Accounts = new HashSet<Account>();
            this.CustomerCoordinators = new HashSet<CustomerCoordinator>();
            this.PhoneNumbers = new HashSet<PhoneNumber>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }
        public string ProvisioningCode { get; set; }
        public string Email { get; set; }
        public bool IsDeleted { get; set; }
    
        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<CustomerCoordinator> CustomerCoordinators { get; set; }
        public virtual ICollection<PhoneNumber> PhoneNumbers { get; set; }
    }
}
