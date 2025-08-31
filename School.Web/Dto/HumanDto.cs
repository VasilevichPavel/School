namespace School.Web.Dto
{
    public class HumanDto
    {
        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public List<int> ClassIds { get; set; } = null!;
    }
}
