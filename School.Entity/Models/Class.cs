using School.Entity.Models.People;

namespace School.Entity.Models
{
    public class Class
    {
        public int Id { get; set; }

        public int TeacherId { get; set; }

        public required Teacher Teacher { get; set; }

        public List<Student> Students { get; set; } = null!;
    }
}
