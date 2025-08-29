namespace Rental_Management_System.Server.DTOs.Tenant
{
    public class TenantDto
    {
        public Guid TenantId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAdress { get; set; }   
        public Guid RoomId { get; set; } // to show assigned room

    }
}
