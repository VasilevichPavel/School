using School.Entity.Models.People;

namespace School.Entity.Models
{
    public class Class
    {
        public int Id { get; set; }

        public required string Name { get; set; } = null!;

        public required int TeacherId { get; set; }

        public Teacher Teacher { get; set; } = null!;

        public List<Student> Students { get; set; } = null!;
    }
}
