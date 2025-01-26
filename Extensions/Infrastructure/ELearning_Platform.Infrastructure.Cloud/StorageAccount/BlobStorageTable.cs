using Azure.Storage.Blobs;


namespace ELearning_Platform.Infrastructure.Cloud.StorageAccount
{
    public class BlobStorageTable(BlobServiceClient blobServiceClient, BlobStorageTablesNames blobStorageTableNames)
    {
        private readonly BlobServiceClient _blobServiceClient = blobServiceClient;

        private readonly BlobStorageTablesNames _blobStorageTableNames = blobStorageTableNames;

        public async Task CreateTable()
        {
            string[] table = [_blobStorageTableNames.profileImage, _blobStorageTableNames.video, _blobStorageTableNames.lessonImage];
            foreach (var item in table)
            {
                var container = _blobServiceClient.GetBlobContainerClient(item);
                if (!await container.ExistsAsync())
                {
                    await container.CreateIfNotExistsAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
                }
            }
            await Task.CompletedTask;
        }
    }
}
