namespace ELearning_Platform.Domain.Models.AccountModel
{
    public class RegisterModelDto
    {
        public string AddressEmail {  get; set; }   
        
        public string Password { get; set; }   
        
        public string ConfirmPassword { get; set; }

        public string FirstName { get; set; }    

        public string? SecondName { get; set; } 

        public string Surname { get; set; } 

        public string PhoneNumber { get; set; } 

        public string City { get; set; }    

        public string Country { get; set; } 

        public string StreetName { get; set; }

        public string PostalCode { get; set; }  
    }
}
