

using DataCatalogService.Models;

namespace DataCatalogService.Repository
{
    public interface IDataCatalogRepository
    {
        List<DataCatalog> Get();
        DataCatalog GetById(string id);
        DataCatalog Create(DataCatalog item);
        void Update(string id, DataCatalog item);
        void Remove(string id);
    }
}

