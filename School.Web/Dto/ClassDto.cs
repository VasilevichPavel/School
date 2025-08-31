using School.Entity.Models.People;

namespace School.Web.Dto
{
    public class CreateClassDto
    {
        public required int TeacherId { get; set; }

        public required string Name { get; set; }

        public List<int> StudentIds { get; set; } = [];
    }

    public class GetClassDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public GetTeacherDto Teacher { get; set; } = null!;

        public List<GetStudentDto> Students { get; set; } = null!;
    }
}
