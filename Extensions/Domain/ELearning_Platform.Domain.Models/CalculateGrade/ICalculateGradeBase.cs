namespace ELearning_Platform.Domain.Models.CalculateGrade
{
    public interface ICalculateGradeBase
    {
        string CalculateGrade(IDictionary<int, int> score);
    }
}
