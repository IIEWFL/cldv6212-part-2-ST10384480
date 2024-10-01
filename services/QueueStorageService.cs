using Azure.Storage.Queues;
using System.Threading.Tasks;

namespace abcRetailFunctionApp.Services
{
    public class QueueStorageService
    {
        private readonly QueueClient _queueClient;

        public QueueStorageService()
        {
            _queueClient = new QueueClient("DefaultEndpointsProtocol=https;AccountName=olebogengdibodusa;AccountKey=O7vHlkH7KU0Q9BKQJ0SGTm+jWE8BTuhOyEUdwXhQn9yTQdDJvBT40nqxmBFndtdvZDJjMMsnYgxO+ASt+mytcA==;EndpointSuffix=core.windows.net", "consumerqueue");
            _queueClient.CreateIfNotExists();
        }

        public async Task SendMessageAsync(string message)
        {
            await _queueClient.SendMessageAsync(message);
        }

        public async Task<string?> ReceiveMessageAsync()
        {
            var response = await _queueClient.ReceiveMessagesAsync(maxMessages: 1);
            if (response.Value.Length > 0)
            {
                var msg = response.Value[0];
                await _queueClient.DeleteMessageAsync(msg.MessageId, msg.PopReceipt);
                return msg.MessageText;
            }
            return null;
        }
    }
}