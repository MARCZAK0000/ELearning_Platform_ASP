﻿namespace ELearning_Platform.Domain.Enitities
{
    public class UserInformations
    {
        public string AccountID { get; set; }

        public Account Account { get; set; }    

        public string FirstName { get; set; }   

        public string? SecondName { get; set; }

        public string Surname { get; set; } 

        public string EmailAddress { get; set; }    

        public string PhoneNumber { get; set; }

        public DateTime ModifiedDate { get; set; } = DateTime.Now;

        public virtual UserAddress Address { get; set; }

        public virtual List<Subject>? Subjects { get; set; }

        public Guid? ClassID {  get; set; }
        
        public virtual ELearningClass? Class { get; set; }

        public virtual List<Lesson>? Lessons { get; set; }

        public virtual List<ELearningClass>? ListOfTeachingClasses { get; set; }

        public virtual List<Notification>? SentNotfications  { get; set; }

        public virtual List<Notification>? RecivedNotifications { get; set; }

    }
}
