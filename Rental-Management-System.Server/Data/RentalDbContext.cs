using Microsoft.EntityFrameworkCore;
using Rental_Management_System.Server.Models;

namespace Rental_Management_System.Server.Data
{
    public class RentalDbContext : DbContext
    {
        public RentalDbContext(DbContextOptions<RentalDbContext> options) : base(options) { }

        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RentPayment> RentPayments { get; set; }

        public DbSet<RentalContract> RentalContracts { get; set; }
        public DbSet<MonthlyCharge> MonthlyCharges { get; set; }
        public DbSet<PaymentTransaction> PaymentTransactions { get; set; }

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
        }
    }

}
