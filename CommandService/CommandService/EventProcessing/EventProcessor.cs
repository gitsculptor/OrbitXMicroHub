using System.Text.Json;
using CommandService.Models;
using CommandService.Repository;

namespace CommandService.EventProcessing;

public class EventProcessor:IEventProcessor
{
    private readonly IServiceScopeFactory _scopeFactory;

    public EventProcessor(IServiceScopeFactory serviceScopeFactory)
    {
        _scopeFactory = serviceScopeFactory;
        

    }
    public void ProcessEvent(string message)
    {
        var eventType = determineEvent(message);
        if (eventType == EventType.Valid)
        {
            AddDataCatalogToCommand(message);
        }
        
    }

    private void AddDataCatalogToCommand(string message)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var repo = scope.ServiceProvider.GetRequiredService<ICommandDataRepository>();
            var dataCatalog = JsonSerializer.Deserialize<DataCatalog>(message);
            try
            {
                if (!repo.ifCommandExistsAlready(dataCatalog.Id))
                {
                    var command = new Command();
                    command.Id = dataCatalog.Id;
                    command.Description = GenerateRandomString(10);
                    repo.CreateCommand(command);
                    Console.WriteLine($"Command Saved Succesfully");
                }
                else
                {
                    Console.WriteLine($"Data catalog already exists");
                }
                    

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }

    private EventType determineEvent(string notficationMessage)
    {
        try
        {
            var eventType = JsonSerializer.Deserialize<DataCatalog>(notficationMessage);
            return EventType.Valid;

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Console.WriteLine($"Unable to deserialize");
            return EventType.Invalid;
        }

    }
    
    private string GenerateRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        Random random = new Random();

        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}

enum EventType
{
    Valid =1,
    Invalid=2
}