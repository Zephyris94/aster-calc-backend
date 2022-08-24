using Azure.Storage.Blobs;
using Core.Settings;
using Infrastructure.Services;
using Microsoft.Extensions.Options;
using System.IO;
using System.Threading.Tasks;

namespace Core.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly DataSourceOptions _settings;

        public BlobStorageService(
            BlobServiceClient blobServiceClient,
            IOptions<DataSourceOptions> dataSourceOptions)
        {
            _blobServiceClient = blobServiceClient;
            _settings = dataSourceOptions.Value;
        }

        public async Task DownloadToStream(MemoryStream ms)
        {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_settings.BlobContainerName);

            BlobClient blobClient = containerClient.GetBlobClient(_settings.DataSourceFile);

            await blobClient.DownloadToAsync(ms);
        }
    }
}
