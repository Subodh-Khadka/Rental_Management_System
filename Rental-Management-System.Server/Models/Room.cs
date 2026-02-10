using System.ComponentModel.DataAnnotations;

namespace Rental_Management_System.Server.Models
{
    public class Room
    {
        [Key]
        public Guid RoomId { get; set; }
        public string RoomTitle { get; set; }
        public decimal RoomPrice { get; set; }

        // Navigation
        public ICollection<RentalContract> RentalContracts { get; set; } = new List<RentalContract>();

        // Soft delete
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedDate { get; set; }

        public bool? IsActive { get; set; } = true;
    }
}


public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Subject> Subjects { get; set; } = new List<Subject>();

}

public class Subject
{
    public int Id { get; set; }
    public string Name { get; set; }

    public int StudentId { get; set; }
    public Student Student { get; set; }
}