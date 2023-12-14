using Microsoft.EntityFrameworkCore;
using bojpawnapi.Entities;
using bojpawnapi.Entities.Auth;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace bojpawnapi.DataAccess
{
    public class PawnDBContext: IdentityDbContext<ApplicationUserEntities> 
    {
        public DbSet<EmployeeEntities> Employees { get; set; }
        public DbSet<CustomerEntities> Customers  { get; set; }
        public DbSet<CollateralTxEntities> CollateralTxs  { get; set; }
        public DbSet<CollateralTxDetailEntities> CollateralTxDetails  { get; set; }
        public PawnDBContext(DbContextOptions<PawnDBContext> options):base(options) 
        {  
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //อันนี้ ถ้าไม่อยากให้มันสร้า่ง เดว Comment ออก
            base.OnModelCreating(modelBuilder);
            //https://stackoverflow.com/questions/36155429/auto-increment-on-partial-primary-key-with-entity-framework-core
            modelBuilder.Entity<CollateralTxDetailEntities>()
                        .HasOne<CollateralTxEntities>(CDtl => CDtl.CollateralTx)
                        .WithMany(C => C.CollateralDetaills)
                        .HasForeignKey(CDtl => CDtl.CollateralId);
                        
            modelBuilder.Entity<CollateralTxEntities>()
                        .HasOne<EmployeeEntities>(C => C.Employee)
                        .WithMany(E => E.CollateralTxls)
                        .HasForeignKey(C => C.EmployeeId);

            modelBuilder.Entity<CollateralTxEntities>()
                        .HasOne<CustomerEntities>(C => C.Customer)
                        .WithMany(C => C.CollateralTxls)
                        .HasForeignKey(C => C.CustomerId);
        }
    }
}