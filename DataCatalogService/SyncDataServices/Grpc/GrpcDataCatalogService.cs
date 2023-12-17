using DataCatalogService.Repository;
using Grpc.Core;

namespace DataCatalogService.SyncDataServices.Grpc;

public class GrpcDataCatalogService : GrpcDataCatalog.GrpcDataCatalogBase
{
    private readonly IDataCatalogRepository _repository;

    public GrpcDataCatalogService(IDataCatalogRepository repo)
    {
        _repository = repo;
        
    }

    public override Task<DataCatalogResponse> GetAllDataCatalog(GetAllRequest request, ServerCallContext context)
    {
        var response = new DataCatalogResponse();
        var dataCatalogs = _repository.Get();

        foreach (var x in dataCatalogs.Select(data => new DataCatalogGrpcModel
                 {
                     Id = data.Id,
                     Cost = data.Cost,
                     Name = data.Name,
                     Publisher = data.Publisher
                 }))
        {
            response.DataCatalog.Add(x);
        }

        return Task.FromResult(response);

    }
    
}