using System.Data.Entity;
using Inspinia_MVC5_SeedProject.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Appa_MVC.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
      
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<File> Files { get; set; }
        public DbSet<ReferanceNumber> ReferanceNumbers { get; set; }
        public DbSet<Vessel> Vessels { get; set; }
        public DbSet<LineItem> LineItems { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<LineItemSupplier> LineItemSuppliers { get; set; }
        public DbSet<EmailLog> EmailLogs { get; set; }
        public DbSet<OfferMaster> OfferMasters { get; set; }
        public DbSet<SupplierPriceLineItem> SupplierPriceLineItems { get; set; }

        public DbSet<SupplierMaster> SupplierMasters { get; set; }

        public DbSet<ViewSupplierPriceLineItem> ViewSupplierPriceLineItems { get; set; }

        public DbSet<CurrencyRate> CurrencyRates { get; set; }

        public DbSet<OrderLineItem> OrderLineItems { get; set; }

        public DbSet<SuppliersCity> SuppliersCities { get; set; }
        public DbSet<RfqMailLog> RfqMailLogs { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<GarretsContract> GarretsContracts { get; set; }

        public DbSet<GARRETS> GARRETSs { get; set; }

        public DbSet<DilovasiMizan> DilovasiMizan { get; set; }
        public DbSet<AntrepoMizan> AntrepoMizan { get; set; }

        public DbSet<VFile> VFiles { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }
    }
}