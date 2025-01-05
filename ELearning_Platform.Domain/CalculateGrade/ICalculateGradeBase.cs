namespace ELearning_Platform.Domain.CalculateGrade
{
    public interface ICalculateGradeBase
    {
        string CalculateGrade(IDictionary<int, int> score);
    }
}
