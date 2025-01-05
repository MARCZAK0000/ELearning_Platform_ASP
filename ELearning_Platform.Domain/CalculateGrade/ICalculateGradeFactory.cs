namespace ELearning_Platform.Domain.CalculateGrade
{
    public interface ICalculateGradeFactory
    {
        ICalculateGradeBase CreateGradeBase(IDictionary<int, int> score);
    }
}
