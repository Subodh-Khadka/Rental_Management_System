using Rental_Management_System.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace Rental_Management_System.Server.Data
{
    public static class DataSeeder
    {
        public static void SeedData(this RentalDbContext context)
        {
            if (!context.Rooms.Any())
            {
                var rooms = new List<Room>
                {
                    new Room { RoomId = Guid.NewGuid(), RoomTitle = "Room 101", RoomPrice = 5000 },
                    new Room { RoomId = Guid.NewGuid(), RoomTitle = "Room 102", RoomPrice = 5500 },
                    new Room { RoomId = Guid.NewGuid(), RoomTitle = "Room 103", RoomPrice = 6000 }
                };

                context.Rooms.AddRange(rooms);
                context.SaveChanges();
            }

            if (!context.Tenants.Any())
            {
                var firstRoom = context.Rooms.First();

                var tenants = new List<Tenant>
                {
                    new Tenant
                    {
                        TenantId = Guid.NewGuid(),
                        Name = "Subodh Khadka",
                        PhoneNumber = "9800000000",
                        EmailAdress = "subodh@example.com",
                        RoomId = firstRoom.RoomId
                    },
                    new Tenant
                    {
                        TenantId = Guid.NewGuid(),
                        Name = "Aayush Shrestha",
                        PhoneNumber = "9811111111",
                        EmailAdress = "aayush@example.com",
                        RoomId = firstRoom.RoomId
                    }
                };

                context.Tenants.AddRange(tenants);
                context.SaveChanges();
            }

            if (!context.RentalContracts.Any())
            {
                var tenant = context.Tenants.First();
                var room = context.Rooms.First();

                var contracts = new List<RentalContract>
                {
                    new RentalContract
                    {
                        RentalContractId = Guid.NewGuid(),
                        TenantId = tenant.TenantId,
                        RoomId = room.RoomId,
                        StartDate = DateTime.Now.AddMonths(-1),
                        Terms = "Standard monthly rent contract"
                    }
                };

                context.RentalContracts.AddRange(contracts);
                context.SaveChanges();
            }

            if (!context.RentPayments.Any())
            {
                var contract = context.RentalContracts.First();
                var room = context.Rooms.First();

                var payments = new List<RentPayment>
                {
                    new RentPayment
                    {
                        PaymentId = Guid.NewGuid(),
                        RentalContractId = contract.RentalContractId,
                        RoomId = room.RoomId,
                        RoomPrice = room.RoomPrice,
                        PaidAmount = 0,
                        PaymentMonth = DateTime.Now
                    }
                };

                context.RentPayments.AddRange(payments);
                context.SaveChanges();
            }
        }
    }
}
