using Microsoft.AspNetCore.Http;

namespace ELearning_Platform.Domain.Repository
{
    public interface IAzureRepository
    {
        public Task<bool> UploadUserImage(byte[] data, string userID, CancellationToken cancellationToken);
    }
}
