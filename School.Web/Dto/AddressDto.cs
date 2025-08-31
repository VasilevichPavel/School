namespace School.Web.Dto
{
    public class UpdateAddressDto : AddressDto { }

    public class GetAddressDto : AddressDto
    {
        public int Id { get; set; }
    }

    public class CreateAddressDto : AddressDto { }

    public class AddressDto
    {
        public required string Street { get; set; }

        public required string City { get; set; }

        public required string PostalCode { get; set; }
    }
}
