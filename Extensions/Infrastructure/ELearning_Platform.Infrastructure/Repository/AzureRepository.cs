﻿using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using ELearning_Platform.Domain.Core.Repository;
using ELearning_Platform.Infrastructure.Cloud.StorageAccount;

namespace ELearning_Platform.Infrastructure.Repository
{
    public class AzureRepository(BlobServiceClient blobServiceClient, BlobStorageTablesNames blobStorageTablesNames) : IAzureRepository
    {
        private readonly BlobServiceClient _blobServiceClient = blobServiceClient;

        private readonly BlobStorageTablesNames _blobStorageTablesNames = blobStorageTablesNames;
        public async Task<bool> UploadUserImage(byte[] file, string userID, CancellationToken token)
        {
            if (file == null || file.Length == 0) return false;
            var container = _blobServiceClient.GetBlobContainerClient(blobContainerName: _blobStorageTablesNames.profileImage);
            await container.DeleteBlobIfExistsAsync(blobName: userID, DeleteSnapshotsOption.None, cancellationToken: token);


            var newBlob = container.GetBlobClient(blobName: userID);

            var blobHeader = new BlobHttpHeaders()
            {
                ContentType = $"image/jpeg"
            };
            using var stream = new MemoryStream(file); //Never use IFormFile in BackgroundService
            await newBlob.UploadAsync(content: stream
            , options: new BlobUploadOptions { HttpHeaders = blobHeader }
            , cancellationToken: token);

            return true;
        }

        public async Task<bool> InsertLessonMaterials(byte[] file, string fileName, string fileType,  CancellationToken token)
        {
            if (file == null || file.Length == 0) return false;
            var container = _blobServiceClient.GetBlobContainerClient(blobContainerName: _blobStorageTablesNames.lessonImage);
            await container.DeleteBlobIfExistsAsync(blobName: fileName, DeleteSnapshotsOption.None, cancellationToken: token);


            var newBlob = container.GetBlobClient(blobName: fileName);

            var blobHeader = new BlobHttpHeaders()
            {
                ContentType = fileType
            };
            using var stream = new MemoryStream(file); //Never use IFormFile in BackgroundService
            await newBlob.UploadAsync(content: stream
            , options: new BlobUploadOptions { HttpHeaders = blobHeader }
            , cancellationToken: token);

            return true;
        }
    }
}
