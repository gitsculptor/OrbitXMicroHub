using CommandService.Models;
using MongoDB.Driver;

namespace CommandService.Repository;

public class CommandDataRepository :ICommandDataRepository
{
    private readonly IMongoCollection<Command> _command;


    public CommandDataRepository(string connectionString,string databaseName)
    {
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);
        _command= database.GetCollection<Command>("Command");
    }

    public async Task CreateCommand(Command command)
    {
        try
        {
            await _command.InsertOneAsync(command);
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in CreateCommand: {ex.Message}");
            throw;
        }
    }

    public async Task<Command> GetCommandById(string id)
    {
        try
        {
            var filter = Builders<Command>.Filter.Eq(c => c.Id, id);
            return await _command.Find(filter).FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            // Handle the exception (e.g., log, throw, etc.)
            Console.WriteLine($"Error in GetCommandById: {ex.Message}");
            throw;
        }
    }

    public bool ifCommandExistsAlready(string dataCatalogId)
    {
        try
        {
            // Use GetCommandById to check if the command already exists
            var existingCommand = GetCommandById(dataCatalogId).Result;

            // If the existingCommand is not null, the command already exists
            return existingCommand != null;
        }
        catch (Exception ex)
        {
            // Handle the exception (e.g., log, throw, etc.)
            Console.WriteLine($"Error in IfCommandExistsAlready: {ex.Message}");
            throw;
        }
    }
}