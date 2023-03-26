// See https://aka.ms/new-console-template for more information
using Azure.Identity;
using Microsoft.Graph;

var tenantId = "<tenantId>";
var clientId = "<clientId>";
var clientSecret = "<clientSecret>";
var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);
var graphClient = new GraphServiceClient(clientSecretCredential);

string siteId = "vmx76.sharepoint.com,2a3a60e5-ff3d-4c30-ae2b-c75dc6d3a10c,0e0bf9d7-d25d-4981-a873-c245b1e88c1a";
string listId = "a9ef9676-65a2-4665-9372-1050932a82c0";

var items = await graphClient.Sites[siteId].Lists[listId].Items.GetAsync((requestConfiguration) =>
{
    requestConfiguration.QueryParameters.Expand = new string[] { "fields" };
});

foreach (var item in items.Value)
{
    // リストの情報を表示する
    Console.WriteLine($"item.Fields: {string.Join(", ", item.Fields.AdditionalData.Select(x => $"{x.Key}: {x.Value}"))}");
}

