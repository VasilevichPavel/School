namespace School.Web.Dto
{
    public class CreateTeacherDto : TeacherDto { }

    public class UpdateTeacherDto : TeacherDto
    {
        public int Id { get; set; }
    }

    public class GetTeacherDto : TeacherDto
    {
        public int Id { get; set; }
    }

    public class TeacherDto : HumanDto { }
}
