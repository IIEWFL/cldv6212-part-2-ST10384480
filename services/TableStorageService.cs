using Azure.Data.Tables;
using abcRetailFunctionApp.Models;
using System.Threading.Tasks;

namespace abcRetailFunctionApp.Services
{
    public class TableStorageService
    {
        private readonly TableClient _tableClient;

        public TableStorageService()
        {
            _tableClient = new TableClient("DefaultEndpointsProtocol=https;AccountName=olebogengdibodusa;AccountKey=O7vHlkH7KU0Q9BKQJ0SGTm+jWE8BTuhOyEUdwXhQn9yTQdDJvBT40nqxmBFndtdvZDJjMMsnYgxO+ASt+mytcA==;EndpointSuffix=core.windows.net", "consumerProfile");
            _tableClient.CreateIfNotExists();
        }

        public async Task UpsertEntityAsync(CustomerProfile customerProfile)
        {
            await _tableClient.UpsertEntityAsync(customerProfile);
        }

        public async Task DeleteEntityAsync(string partitionKey, string rowKey)
        {
            await _tableClient.DeleteEntityAsync(partitionKey, rowKey);
        }
    }
}