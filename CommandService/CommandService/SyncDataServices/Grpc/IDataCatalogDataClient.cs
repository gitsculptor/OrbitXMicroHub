using CommandService.Models;

namespace CommandService.SyncDataServices.Grpc;

public interface IDataCatalogDataClient
{
    IEnumerable<DataCatalog> GetAllDataCatalogs();
}