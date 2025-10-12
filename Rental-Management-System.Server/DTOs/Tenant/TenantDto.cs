namespace Rental_Management_System.Server.DTOs.Tenant
{
    public class TenantDto
    {
        public Guid TenantId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }   
        public Guid RoomId { get; set; } // to show assigned room
        public string RoomTitle { get; set; } // Add this
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }

    }
}
