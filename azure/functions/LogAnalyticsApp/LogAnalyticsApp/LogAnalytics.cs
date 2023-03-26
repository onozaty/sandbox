using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Monitor.Query.Models;
using Azure.Monitor.Query;
using Azure;
using Azure.Identity;
using System.Text;

namespace LogAnalyticsApp
{
    public static class LogAnalytics
    {
        [FunctionName("LogAnalytics")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string workspaceId = Environment.GetEnvironmentVariable("WORKSPACE_ID");
            string query = "MicrosoftAzureBastionAuditLogs";
            QueryTimeRange timeRange = new QueryTimeRange(TimeSpan.FromDays(7));

            var client = new LogsQueryClient(new DefaultAzureCredential());
            Response<LogsQueryResult> response = await client.QueryWorkspaceAsync(
                workspaceId,
                query,
                timeRange);

            StringBuilder stringBuilder = new StringBuilder();

            LogsTable table = response.Value.Table;

            foreach (var row in table.Rows)
            {
                stringBuilder.AppendLine(row["Time"] + " " + row["OperationName"]);
            }

            return new OkObjectResult(stringBuilder.ToString());

        }
    }
}
