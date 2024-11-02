﻿namespace ELearning_Platform.Domain.Enitities
{
    public class Subject
    {
        public Guid SubjectId { get; set; } 

        public Guid ClassID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string TeacherID {  get; set; }
        
        public string TeacherName { get; set; }

        public string TeacherSurname { get; set; }  

        public List<Grade> Grades { get; set; }

        public UserInformations Teacher {  get; set; }

        public ELearningClass Class { get; set; }

        public List<Lesson>? Lessons { get; set; }

        public List<Test> Tests { get; set; }

        public DateTime ModifiedDate { get; set; } = DateTime.Now;
    }
}
