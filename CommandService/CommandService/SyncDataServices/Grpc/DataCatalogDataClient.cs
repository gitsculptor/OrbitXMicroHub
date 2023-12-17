using CommandService.Models;
using DataCatalogService;
using Grpc.Net.Client;

namespace CommandService.SyncDataServices.Grpc;

public class DataCatalogDataClient :IDataCatalogDataClient
{
    private readonly IConfiguration _configuration;

    public DataCatalogDataClient(IConfiguration config)
    {
        _configuration = config;

    }
    
    
    public IEnumerable<DataCatalog> GetAllDataCatalogs()
    {
        Console.WriteLine($"-->> calling grpc Service{_configuration["GrpcDataCatalog"]} ");
    
        var channel = GrpcChannel.ForAddress(_configuration["GrpcDataCatalog"]);
        var client = new GrpcDataCatalog.GrpcDataCatalogClient(channel);
        var request = new GetAllRequest();
        try
        {
            var reply = client.GetAllDataCatalog(request);
            List<DataCatalog> result = new List<DataCatalog>();
            foreach (var data in reply.DataCatalog)
            {
                var data2 = MapDataCatalog(data);
                result.Add(data2);
            }
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine($"error in grpc {e.Message}");
            return null;
        }
        
        throw new NotImplementedException();
    }
    // public IEnumerable<DataCatalog> GetAllDataCatalogs()
    // {
    //     throw new NotImplementedException();
    // }
    private DataCatalog MapDataCatalog(DataCatalogGrpcModel grpcModel)
    {
        // Manual mapping from DataCatalogGrpcModel to DataCatalog
        return new DataCatalog
        {
            Id = grpcModel.Id,
            Name = grpcModel.Name,
            Publisher = grpcModel.Publisher,
            Cost = grpcModel.Cost
            // Map other properties as needed
        };
    }
}