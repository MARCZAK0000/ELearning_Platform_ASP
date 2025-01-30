using ELearning_Platform.Domain.Database.Enitities;
using ELearning_Platform.Domain.Database.EntitiesDto;
using ELearning_Platform.Domain.Database.EntitiesMapper;

namespace ELearning_Platform.Application.Mapper
{
    public static class PlatformDbMapper
    {
        #region User Dto 
        /// <summary>
        /// Extension method of class User, used to create UserDto
        /// </summary>
        /// <returns>User Dto</returns>
        public static UserDto UserToDto(this User user)
        {
            return UserToDto(user, false);
        }


        /// <summary>
        /// Extension method of class User, used to create UserDto and UserAddressDto
        /// </summary>
        /// <returns>User Dto with UserAddressDto</returns>
        public static UserDto UserToDto(this User user, bool includeAddress)
        {
            return new UserDto
            {
                AccountID = user.AccountID,
                EmailAddress = user.EmailAddress,
                PhoneNumber = user.PhoneNumber,
                FirstName = user.FirstName,
                Surname = user.Surname,
                SecondName = user.SecondName ?? string.Empty,
                UserAddress = includeAddress ? user.Address.ToDto() : EmptyObject<UserAddressDto>.Empty()
            };
        }
        /// <summary>
        /// Extension method of class UserAddress, used to create DTO
        /// </summary>
        /// <returns>return UserAddress Dto</returns>
        public static UserAddressDto ToDto(this UserAddress userAddress)
        {
            if (userAddress is null)
                return new UserAddressDto { };
            return new UserAddressDto
            {
                City = userAddress.City,
                Country = userAddress.Country,
                PostalCode = userAddress.PostalCode,
                StreetName = userAddress.StreetName,
            };
        }
        #endregion

        #region Elearning Class 

        public static ElearningClassDto ClassToDto(this ELearningClass eLearningClass)
        {
            return ClassToDto(eLearningClass, false, false, false);
        }

        public static ElearningClassDto ClassToDto(this ELearningClass eLearningClass, bool includeStudents, bool includeTeachers, bool includeSubjects)
        {
            return new ElearningClassDto
            {
                ElearningClassID = eLearningClass.ELearningClassID,
                Name = eLearningClass.Name,
                YearOfBeggining = eLearningClass.YearOfBeggining,
                YearOfEnding = eLearningClass.YearOfEnding,
                Students = includeStudents ? eLearningClass.Students?.Select(pr => pr.StudentToDto()).ToList() : [],
                Subjects = includeSubjects ? eLearningClass.Subjects?.Select(pr=>pr.SubjectToDto()).ToList(): [],
                Teachers = includeTeachers ? eLearningClass.Teachers?.Select(pr => pr.TeacherToDto()).ToList() : []
            };
        }

        #endregion

        #region Students
        public static StudentsDto StudentToDto(this Students students)
        {
            return new StudentsDto
            {
                Student = students.User.UserToDto()
            };
        }
        #endregion

        #region Teachers 
        public static TeacherDto TeacherToDto(this Teachers teachers)
        {
            return new TeacherDto
            {
                Teacher = teachers.User.UserToDto()
            };
        }
        #endregion

        #region Subject
        public static SubjectDto SubjectToDto(this Subject subject)
        {
            return SubjectToDto(subject, false);
        }
        public static SubjectDto SubjectToDto(this Subject subject, bool includeLessons)
        {
            return new SubjectDto
            {

            };
        }
        #endregion

        #region 
        public static LessonDto LessonToDto(this Lesson lesson)
        {
            return LessonToDto(lesson, false);
        }

        public static LessonDto LessonToDto(this Lesson lesson, bool includeMaterials)
        {
            return new LessonDto
            {

            };
        }
        #region LessonMaterialsDto 

        #endregion

        #region TestDto 
        public static TestDto TestToDto(this Test test)
        {

        }
        #endregion
    }
}

