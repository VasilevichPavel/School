namespace School.Web.Dto
{
    public class GetStudentDto : StudentDto
    {
        public int Id { get; set; }

        public List<int> ClassIds { get; set; } = null!;
    }

    public class UpdateStudentDto : StudentDto
    {
        public required int Id { get; set; }

        public UpdateAddressDto? Address { get; set; }
    }

    public class CreateStudentDto : StudentDto
    {
        public CreateAddressDto? Address { get; set; }
    }

    public class StudentDto : HumanDto
    {
        public required DateTime DayOfBirth { get; set; }
    }
}
