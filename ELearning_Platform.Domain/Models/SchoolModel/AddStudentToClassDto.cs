﻿namespace ELearning_Platform.Domain.Models.SchoolModel
{
    public class AddStudentToClassDto
    {
        public string ClassID { get; set; }

        public List<string> UsersToAdd { get; set; }
    }
}
