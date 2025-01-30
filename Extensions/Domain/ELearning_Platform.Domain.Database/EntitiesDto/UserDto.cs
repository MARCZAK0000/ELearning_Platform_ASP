using ELearning_Platform.Domain.Database.Enitities;
using ELearning_Platform.Domain.Database.EntitiesDto;

namespace ELearning_Platform.Domain.Database.EntitiesMapper
{
    public class UserDto
    {
        public string AccountID { get; set; }

        public string FirstName { get; set; }

        public string? SecondName { get; set; }

        public string Surname { get; set; }

        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }

        public UserAddressDto UserAddress { get; set; }
    }
}
