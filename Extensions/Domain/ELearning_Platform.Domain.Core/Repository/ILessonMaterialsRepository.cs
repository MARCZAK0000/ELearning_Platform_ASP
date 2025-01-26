using Microsoft.AspNetCore.Http;

namespace ELearning_Platform.Domain.Core.Repository
{
    public interface ILessonMaterialsRepository
    {
        Task<bool> AddLessonMaterialsAsync(List<IFormFile> files, string lessonId, CancellationToken token);
    }
}
