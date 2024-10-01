using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using System.Text.Json;

public class WriteToBlobFunction
{
    [Function("WriteToBlob")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "writetoblob")] HttpRequestData req)
    {
        // Read the request body manually
        string requestBody;
        using (var reader = new StreamReader(req.Body))
        {
            requestBody = await reader.ReadToEndAsync();
        }

        // Deserialize the request body (assuming it's JSON)
        var data = JsonSerializer.Deserialize<YourModel>(requestBody);

        // Write the data to Blob Storage (pseudo code here)
        // You will need to call your BlobStorageService and upload the data

        // Return response
        var response = req.CreateResponse();
        response.StatusCode = System.Net.HttpStatusCode.OK;
        await response.WriteStringAsync("Data written to blob successfully.");
        return response;
    }
}

public class YouModel
{
    public string? Property1 { get; set; }
    public string? Property2 { get; set; }
}