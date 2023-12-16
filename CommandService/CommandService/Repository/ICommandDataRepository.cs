using CommandService.Models;

namespace CommandService.Repository;

public interface ICommandDataRepository
{
    Task CreateCommand(Command command);
    Task<Command> GetCommandById(string id);
    bool ifCommandExistsAlready(string dataCatalogId);
}