using System.Text;
using System.Text.Json;
using DataCatalogService.Models;

namespace DataCatalogService.SyncDataServices.Http;

public class CommandDataClient :ICommandDataClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public CommandDataClient(HttpClient _httpClient,IConfiguration _configuration)
    {
        this._httpClient = _httpClient;
        this._configuration = _configuration;
    }
    public async  Task SendDataCatalogToCommand(DataCatalog data)
    {
        var httpContent = new StringContent(
            JsonSerializer.Serialize(data),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _httpClient.PostAsync(_configuration["CommandService"], httpContent);
        
        if(response.IsSuccessStatusCode)
            Console.WriteLine("--> Sync POST to service 2 was Ok");
        else
        {
            Console.WriteLine(("-->Sync POST to service 2 was failed"));
        }
    }
}