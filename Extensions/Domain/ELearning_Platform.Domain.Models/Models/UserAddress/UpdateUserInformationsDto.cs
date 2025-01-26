namespace ELearning_Platform.Domain.Models.Models.UserAddress
{
    public class UpdateUserInformationsDto
    {
        public string FirstName { get; set; }

        public string Surname { get; set; }

        public string? SecondName { get; set; }

        public string PhoneNumber { get; set; }

        public UpdateAddressDto Address { get; set; }
    }
}
