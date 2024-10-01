using abcRetailFunctionApp.Models;
using abcRetailFunctionApp.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace abcRetailFunctionApp.Functions
{
    public class StoreToTableFunction
    {
        private readonly TableStorageService _tableStorageService;
        private readonly ILogger<StoreToTableFunction> _logger;

        public StoreToTableFunction(TableStorageService tableStorageService, ILogger<StoreToTableFunction> logger)
        {
            _tableStorageService = tableStorageService;
            _logger = logger;
        }

        [Function("StoreToTable")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "storetotable")] HttpRequestData req)
        {
            try
            {
                var customerProfile = await req.ReadFromJsonAsync<CustomerProfile>();
                if (customerProfile == null)
                {
                    var badResponse = req.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                    await badResponse.WriteStringAsync("Invalid data.");
                    return badResponse;
                }

                // Assign PartitionKey and RowKey if not set
                customerProfile.PartitionKey = "CustomerProfile";
                customerProfile.RowKey = Guid.NewGuid().ToString();

                await _tableStorageService.UpsertEntityAsync(customerProfile);

                var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
                await response.WriteStringAsync("Customer profile saved to table.");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error storing to table.");
                var errorResponse = req.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
                await errorResponse.WriteStringAsync("Internal server error.");
                return errorResponse;
            }
        }
    }
}