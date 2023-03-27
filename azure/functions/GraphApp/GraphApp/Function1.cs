using Azure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using System.Text;
using System.Threading.Tasks;

namespace GraphApp
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var graphClient = new GraphServiceClient(new DefaultAzureCredential());

            var result = await graphClient.Me.GetAsync();

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"DisplayName: {result.DisplayName}");
            stringBuilder.AppendLine($"CompanyName: {result.CompanyName}");
            stringBuilder.AppendLine($"Mail: {result.Mail}");

            return new OkObjectResult(stringBuilder.ToString());
        }
    }
}
