namespace School.Entity.Models.People
{
    public class Student : Human
    {
        public string StudentId { get; set; } = string.Empty;

        public required DateTime DayOfBirth { get; set; }

        public int? AddressId { get; set; }

        public Address? Address { get; set; }
    }
}
