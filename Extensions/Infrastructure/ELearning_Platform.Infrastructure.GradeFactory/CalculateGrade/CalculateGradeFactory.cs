using ELearning_Platform.Domain.Models.CalculateGrade;
using ELearning_Platform.Domain.Setttings.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace ELearning_Platform.Infrastructure.GradeFactory.CalculateGrade
{
    public class CalculateGradeFactory(IServiceProvider serviceProvider,
        GradeInformations gradeInformations) : ICalculateGradeFactory
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        private readonly GradeInformations _gradeInformations = gradeInformations;

        public ICalculateGradeBase CreateGradeBase()
        {
            return _gradeInformations.Country switch
            {
                "Poland" => _serviceProvider.GetRequiredService<PolandCalculateGrade>(),
                "USA" => _serviceProvider.GetRequiredService<USACalculateGrade>(),
                "Germany" => _serviceProvider.GetRequiredService<GermanCalculateGrade>(),
                _ => throw new NotSupportedException(),
            };
        }
    }
}
