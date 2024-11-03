using ELearning_Platform.Domain.CalculateGrade;
using ELearning_Platform.Domain.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace ELearning_Platform.Infrastructure.CalculateGrade
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
                _=> throw new NotSupportedException(),
            };
        }
    }
}
