using DataCatalogService.Models;
using MongoDB.Driver;

namespace DataCatalogService.Repository;

public class DataCatalogRepo: IDataCatalogRepository
{
    private readonly IMongoCollection<DataCatalog> _todos;

    public DataCatalogRepo(string connectionString,string databaseName)
    {
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);
        _todos = database.GetCollection<DataCatalog>("Datacatalog");
    }
    
    public List<DataCatalog> Get() =>
        _todos.Find(item => true).ToList();

    public DataCatalog GetById(string id) =>
        _todos.Find<DataCatalog>(item => item.Id == id).FirstOrDefault();

    public DataCatalog Create(DataCatalog item)
    {
        item.Id = Guid.NewGuid().ToString();
        _todos.InsertOne(item);
        return item;
    }
    
    public void Update(string id, DataCatalog item) =>
        _todos.ReplaceOne(i => i.Id == id, item);

    

    public void Remove(string id) =>
        _todos.DeleteOne(i => i.Id == id);
}