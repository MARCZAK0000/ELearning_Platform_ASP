using ELearning_Platform.Domain.BackgroundTask;
using ELearning_Platform.Domain.Models.SchoolModel;
using ELearning_Platform.Domain.Repository;
using ELearning_Platform.Infrastructure.BackgroundStrategy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;

namespace ELearning_Platform.Infrastructure.Repository
{
    public class LessonMaterialsRepository(BackgroundTask backgroundTask
        , IAzureHandlerQueue azureHandlerQueue) : ILessonMaterialsRepository
    {
        private readonly BackgroundTask _backgroundTask = backgroundTask;
        private readonly IAzureHandlerQueue _azureHandlerQueue = azureHandlerQueue;
        public async Task<bool> AddLessonMaterialsAsync(List<IFormFile> files, string lessonID, CancellationToken token)
        {
            var insert = new InsertFilesToCloudDto
            {
                LessonId = lessonID
            };
            insert.Materials ??= [];
            foreach (var item in files)
            {
                using var memoryStrem = new MemoryStream();
                item.CopyTo(memoryStrem);
                var data = memoryStrem.ToArray();
                insert.Materials.Add(new MaterialFiles()
                {
                    Data = data, Name = $"{lessonID}_{item.FileName}_{insert.Materials.Count}", Type = item.ContentType
                });
            }
            _azureHandlerQueue.QueueBackgroundWorkItem(async =>
                _backgroundTask.ExecuteTask(BackgroundEnum.File, insert, token)
            );

            return await Task.FromResult(true);
        }
    }
}
