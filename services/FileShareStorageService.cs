using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;
using System.IO;
using System.Threading.Tasks;

namespace abcRetailFunctionApp.Services
{
    public class FileShareStorageService
    {
        private readonly ShareClient _shareClient;

        public FileShareStorageService()
        {
            _shareClient = new ShareClient("DefaultEndpointsProtocol=https;AccountName=olebogengdibodusa;AccountKey=O7vHlkH7KU0Q9BKQJ0SGTm+jWE8BTuhOyEUdwXhQn9yTQdDJvBT40nqxmBFndtdvZDJjMMsnYgxO+ASt+mytcA==;EndpointSuffix=core.windows.net", "consumerfileshare");
            _shareClient.CreateIfNotExists();
        }

        public async Task UploadFileAsync(Stream fileStream, string fileName)
        {
            var directoryClient = _shareClient.GetRootDirectoryClient();
            var fileClient = directoryClient.GetFileClient(fileName);
            await fileClient.CreateAsync(fileStream.Length);
            await fileClient.UploadRangeAsync(new Azure.HttpRange(0, fileStream.Length), fileStream);
        }
    }
}