using ELearning_Platform.Domain.Models.Models.UserAddress;

namespace ELearning_Platform.Domain.Models.Response.UserReponse
{
    public class GetUserInformationsDto
    {
        public string AccountID { get; set; }

        public string FirstName { get; set; }

        public string? SecondName { get; set; }

        public string Surname { get; set; }

        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }

        public string RoleName { get; set; }

        public UserAddressDto Address { get; set; }

        public string? ClassName { get; set; }
    }
}
