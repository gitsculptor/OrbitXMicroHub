using DataCatalogService.Models;

namespace DataCatalogService.SyncDataServices.Http;

public interface ICommandDataClient
{
    Task SendDataCatalogToCommand(DataCatalog data);
}