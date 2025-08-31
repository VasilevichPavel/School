namespace School.Entity.Models.People
{
    public abstract class Human
    {
        public int Id { get; set; }

        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public List<Class> Classes { get; set; } = null!;
    }
}
