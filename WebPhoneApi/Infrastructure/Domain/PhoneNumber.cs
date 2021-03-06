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
    
    public partial class PhoneNumber
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public Nullable<bool> HasE911 { get; set; }
        public string E911 { get; set; }
        public Nullable<int> PhoneNumberProviderId { get; set; }
        public bool IsDeleted { get; set; }
    
        public virtual Customer Customer { get; set; }
        public virtual PhoneNumberProvider PhoneNumberProvider { get; set; }
    }
}
