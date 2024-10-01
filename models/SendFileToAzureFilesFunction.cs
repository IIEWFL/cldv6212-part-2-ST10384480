using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using System.Text.Json;

public class SendFileToAzureFilesFunction
{
    [Function("SendFileToAzureFiles")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequestData req)
    {
        string requestBody;
        using (StreamReader reader = new StreamReader(req.Body))
        {
            requestBody = await reader.ReadToEndAsync();
        }

        // Parse the body (assuming JSON input)
        var data = JsonSerializer.Deserialize<YourModel>(requestBody);

        // Your logic to send the file to Azure Files

        var response = req.CreateResponse();
        response.StatusCode = System.Net.HttpStatusCode.OK;
        await response.WriteStringAsync("File sent to Azure successfully.");
        return response;
    }
}

public class YourModel
{
    public string Property1 { get; set; }
    public string Property2 { get; set; }
}