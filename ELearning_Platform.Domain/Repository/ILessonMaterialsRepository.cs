using Microsoft.AspNetCore.Http;

namespace ELearning_Platform.Domain.Repository
{
    public interface ILessonMaterialsRepository
    {
        Task <bool> AddLessonMaterialsAsync(List<IFormFile> files, Guid lessonId, CancellationToken token);
    }
}
