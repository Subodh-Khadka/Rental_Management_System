using Microsoft.EntityFrameworkCore;
using Rental_Management_System.Server.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Rental_Management_System.Server.Data
{
    public class RentalDbContext : IdentityDbContext<ApplicationUser>
    {
        public RentalDbContext(DbContextOptions<RentalDbContext> options) : base(options) { }

        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RentPayment> RentPayments { get; set; }

        public DbSet<RentalContract> RentalContracts { get; set; }
        public DbSet<MonthlyCharge> MonthlyCharges { get; set; }
        public DbSet<PaymentTransaction> PaymentTransactions { get; set; }
        public DbSet<ChargeTemplate> ChargeTemplates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // RentPayment → Room, no cascade delete
            modelBuilder.Entity<RentPayment>()
                .HasOne(rp => rp.Room)
                .WithMany()
                .HasForeignKey(rp => rp.RoomId)
                .OnDelete(DeleteBehavior.Restrict);

            // RentPayment → RentalContract, optional restrict
            modelBuilder.Entity<RentPayment>()
                .HasOne(rp => rp.RentalContract)
                .WithMany(rc => rc.Payments)
                .HasForeignKey(rp => rp.RentalContractId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Tenant>()
        .HasOne(t => t.Room)
        .WithMany() // or .WithMany(r => r.Tenants) if navigation exists
        .HasForeignKey(t => t.RoomId)
        .OnDelete(DeleteBehavior.NoAction); // <-- prevent cascade
        }

    }

}
