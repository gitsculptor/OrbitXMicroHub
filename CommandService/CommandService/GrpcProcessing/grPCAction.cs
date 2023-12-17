using CommandService.Models;
using CommandService.Repository;
using CommandService.SyncDataServices.Grpc;

namespace CommandService.GrpcProcessing;

public static class grPCAction
{
    public static void PrepPopulation(ApplicationBuilder applicationBuilder)
    {
        using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
        {
            var grpcClient = serviceScope.ServiceProvider.GetService<IDataCatalogDataClient>();

            var datacatalogs = grpcClient.GetAllDataCatalogs();
            SeedData(serviceScope.ServiceProvider.GetService<ICommandDataRepository>(),datacatalogs);
            
        }
    }

    private static void SeedData(ICommandDataRepository repo, IEnumerable<DataCatalog> datas)
    {
        Console.WriteLine($"Seeding New data to Commands->>");

        foreach (var data in datas)
        {
            if (!repo.ifCommandExistsAlready(data.Id))
            {
                Console.WriteLine($"Seeding to comand ->>");
                var command = new Command
                {
                    Id = data.Id,
                    Description = $"Created via Grpc: {GenerateRandomString(5)}"
                };
                repo.CreateCommand(command);
            }
            Console.WriteLine($"Already exists not seeding ->>");
        }
        
    }
    
    private static string GenerateRandomString(int length)
     {
         const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
         Random random = new Random();

         return new string(Enumerable.Repeat(chars, length)
             .Select(s => s[random.Next(s.Length)]).ToArray());
     }
}