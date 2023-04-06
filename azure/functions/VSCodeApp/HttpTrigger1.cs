using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using System.Text;
using Azure.Identity;

namespace Company.Function
{
    public static class HttpTrigger1
    {
        [FunctionName("HttpTrigger1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var tenantId = Environment.GetEnvironmentVariable("TENANT_ID");
            var clientId = Environment.GetEnvironmentVariable("CLIENT_ID");
            var clientSecret = Environment.GetEnvironmentVariable("CLIENT_SECRET");

            var clientSecretCredential = new ClientSecretCredential(
                tenantId, clientId, clientSecret);

            var graphClient = new GraphServiceClient(clientSecretCredential);

            var result = await graphClient.Users.GetAsync();

            StringBuilder stringBuilder = new StringBuilder();
            foreach(var user in result.Value) {
                stringBuilder.AppendLine($"{user.Mail}: {user.DisplayName}");
            }

            return new OkObjectResult(stringBuilder.ToString());
        }
    }
}
