using abcRetailFunctionApp.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace abcRetailFunctionApp.Functions
{
    public class QueueOperationsFunction
    {
        private readonly QueueStorageService _queueStorageService;
        private readonly ILogger<QueueOperationsFunction> _logger;

        public QueueOperationsFunction(QueueStorageService queueStorageService, ILogger<QueueOperationsFunction> logger)
        {
            _queueStorageService = queueStorageService;
            _logger = logger;
        }

        [Function("WriteToQueue")]
        public async Task<HttpResponseData> WriteToQueue(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "writetoqueue")] HttpRequestData req)
        {
            try
            {
                var message = await req.ReadAsStringAsync();
                if (string.IsNullOrEmpty(message))
                {
                    var badResponse = req.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    await badResponse.WriteStringAsync("No message provided.");
                    return badResponse;
                }

                await _queueStorageService.SendMessageAsync(message);

                var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
                await response.WriteStringAsync("Message written to queue.");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error writing to queue.");
                var errorResponse = req.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
                await errorResponse.WriteStringAsync("Internal server error.");
                return errorResponse;
            }
        }

        [Function("ReadFromQueue")]
        public async Task<HttpResponseData> ReadFromQueue(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "readfromqueue")] HttpRequestData req)
        {
            try
            {
                var message = await _queueStorageService.ReceiveMessageAsync();
                if (message == null)
                {
                    var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
                    await response.WriteStringAsync("No messages in queue.");
                    return response;
                }

                var successResponse = req.CreateResponse(System.Net.HttpStatusCode.OK);
                await successResponse.WriteStringAsync($"Message: {message}");
                return successResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading from queue.");
                var errorResponse = req.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
                await errorResponse.WriteStringAsync("Internal server error.");
                return errorResponse;
            }
        }
    }
}