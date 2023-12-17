using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommandService.Models;
using CommandService.Repository;
using CommandService.SyncDataServices.Grpc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CommandService.GrpcProcessing
{
    public class GpcScopedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public GpcScopedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dataCatalogService = scope.ServiceProvider.GetRequiredService<GpcScopedService>();
                await dataCatalogService.ProcessDataCatalogsAsync();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Cleanup logic, if needed
            return Task.CompletedTask;
        }

        private async Task ProcessDataCatalogsAsync()
        {
            var dataCatalogDataClient = _serviceProvider.GetRequiredService<IDataCatalogDataClient>();
            var commandDataRepository = _serviceProvider.GetRequiredService<ICommandDataRepository>();

            // Get data catalogs from gRPC

            try
            {
                var grpcDataCatalogs = dataCatalogDataClient.GetAllDataCatalogs();

                if (grpcDataCatalogs != null)
                {
                    foreach (var grpcDataCatalog in grpcDataCatalogs)
                    {
                        // Check if the data catalog is present in the database
                        if (!commandDataRepository.ifCommandExistsAlready(grpcDataCatalog.Id))
                        {
                            var command = new Command
                            {
                                Id = grpcDataCatalog.Id,
                                Description = $"GRPC generated -> {GenerateRandomString(5)}"
                            };
                            
                            await commandDataRepository.CreateCommand(command);
                            Console.WriteLine($"gRPC genrated command in Db inserted {grpcDataCatalog.Id}");
                        }
                        else
                        {
                            Console.WriteLine($"Already exists in db");
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Error in grpc calling");
            }
            
        }

        private string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            var random = new Random();

            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
