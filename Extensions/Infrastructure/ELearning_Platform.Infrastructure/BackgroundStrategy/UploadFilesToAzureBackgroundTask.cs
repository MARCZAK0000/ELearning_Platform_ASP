using ELearning_Platform.Domain.Core.BackgroundTask;
using ELearning_Platform.Domain.Core.Repository;
using ELearning_Platform.Domain.Models.Models.SchoolModel;
using ELearning_Platform.Domain.Models.SchoolModel;

namespace ELearning_Platform.Infrastructure.BackgroundStrategy
{
    public class UploadFilesToAzureBackgroundTask(IAzureRepository azureRepository) : IBackgroundTask
    {
        private readonly IAzureRepository _azureRepository = azureRepository;
        public async Task ExecuteAsync(object parametrs, CancellationToken token)
        {
            var dto = parametrs as InsertFilesToCloudDto;
            if(dto is null)
            {
                ArgumentException.ThrowIfNullOrEmpty(nameof(dto));
            }

            foreach (var item in dto!.Materials)
            {
                await _azureRepository.InsertLessonMaterials(item.Data, item.Name, item.Type, token);
            }
        }
    }
}
