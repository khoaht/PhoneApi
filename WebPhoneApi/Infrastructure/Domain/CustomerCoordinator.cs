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
    
    public partial class CustomerCoordinator
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Coordinator1 { get; set; }
        public bool IsDeleted { get; set; }
        public string Extension { get; set; }
        public string Phone { get; set; }
    
        public virtual Customer Customer { get; set; }
    }
}