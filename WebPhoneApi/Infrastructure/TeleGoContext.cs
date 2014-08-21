using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;


namespace Infrastructure.Domain
{
    public partial class TeleGoContext : DbContext
    {

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Account>()
                .ToTable("Account");
            modelBuilder.Entity<Customer>()
                .ToTable("Customer");

            modelBuilder.Entity<CustomerCoordinator>()
                .ToTable("CustomerCoordinator");
            modelBuilder.Entity<PhoneNumber>()
                .ToTable("PhoneNumber");
            modelBuilder.Entity<PhoneNumberProvider>()
                .ToTable("PhoneNumberProvider");
            modelBuilder.Entity<User>()
                .ToTable("User");
        }
    }
}
