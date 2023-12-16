using DataCatalogService.Models;

namespace DataCatalogService.AsyncDataServices;

public interface IMessageBusClient
{
    void PublishNewDataCatalog(DataCatalog data);

}