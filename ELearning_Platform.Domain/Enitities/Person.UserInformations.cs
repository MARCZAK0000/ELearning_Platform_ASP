﻿namespace ELearning_Platform.Domain.Enitities
{
    public class UserInformations
    {
        public string AccountID { get; set; }

        public Account Account { get; set; }    

        public string FirstName { get; set; }   

        public string SecondName { get; set; }

        public string Surname { get; set; } 

        public string EmailAddress { get; set; }    

        public string PhoneNumber { get; set; }

        public DateOnly ModifidedDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        public virtual UserAddress Address { get; set; }

    }
}