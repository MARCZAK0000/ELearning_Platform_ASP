using ELearning_Platform.Domain.Exceptions.Exceptions;
using ELearning_Platform.Domain.Models.CalculateGrade;
using ELearning_Platform.Domain.Setttings.Settings;

namespace ELearning_Platform.Infrastructure.GradeFactory.CalculateGrade
{
    public class PolandCalculateGrade(GradePercentage gradePercentage, GradeConversion gradeConversion) : ICalculateGradeBase
    {
        private readonly GradePercentage _gradePercentage = gradePercentage;
        private readonly GradeConversion _gradeConversion = gradeConversion;
        public string CalculateGrade(IDictionary<int, int> score)
        {
            var gradeScore = score.Keys.FirstOrDefault();
            var gradePointScore = score.Values.FirstOrDefault();

            if (gradePointScore == 0)
            {
                throw new InternalServerErrorException("Grade point score cannot be zero");
            }

            var calculatePercentage = gradeScore / gradePointScore * 100;
            if (calculatePercentage <= _gradePercentage.Grade_VeryBad)
            {
                return _gradeConversion.Grade_VeryBad;
            }
            else if (calculatePercentage <= _gradePercentage.Grade_Bad)
            {
                return _gradeConversion.Grade_Bad;
            }
            else if (calculatePercentage <= _gradePercentage.Grade_Medium)
            {
                return _gradeConversion.Grade_Medium;
            }
            else if (calculatePercentage <= _gradePercentage.Grade_Good)
            {
                return _gradeConversion.Grade_Good;
            }
            else if (calculatePercentage <= _gradePercentage.Grade_VeryGood)
            {
                return _gradeConversion.Grade_VeryGood;
            }
            else if (calculatePercentage <= _gradePercentage.Grade_Perfect)
            {
                return _gradeConversion.Grade_Perfect;
            }
            else
            {
                throw new InternalServerErrorException("Problem with grade calculations");
            }
        }
    }
}
