using Azure.Storage.Blobs;
using System.IO;
using System.Threading.Tasks;

namespace abcRetailFunctionApp.Services
{
    public class BlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _blobContainerName;

        public BlobStorageService()
        {
            _blobServiceClient = new BlobServiceClient("DefaultEndpointsProtocol=https;AccountName=olebogengdibodusa;AccountKey=O7vHlkH7KU0Q9BKQJ0SGTm+jWE8BTuhOyEUdwXhQn9yTQdDJvBT40nqxmBFndtdvZDJjMMsnYgxO+ASt+mytcA==;EndpointSuffix=core.windows.net");
            _blobContainerName = "consumerblob";
        }

        public async Task UploadFileAsync(Stream fileStream, string fileName)
        {
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(_blobContainerName);
            await blobContainerClient.CreateIfNotExistsAsync();

            var blobClient = blobContainerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(fileStream, overwrite: true);
        }
    }
}