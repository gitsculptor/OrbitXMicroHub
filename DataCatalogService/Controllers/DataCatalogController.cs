using DataCatalogService.AsyncDataServices;
using DataCatalogService.Models;
using DataCatalogService.Repository;
using DataCatalogService.SyncDataServices.Http;
using Microsoft.AspNetCore.Mvc;

namespace DataCatalogService.Controllers
{
    [ApiController]
    [Route("api/data_catalog")]
    public class DataCatalogController : ControllerBase
    {
        private readonly IDataCatalogRepository _repository;
        private readonly ICommandDataClient _commandDataClient;
        private readonly IMessageBusClient _messageBusClient;

        public DataCatalogController(IDataCatalogRepository repository,ICommandDataClient _commandDataClient,IMessageBusClient messageBusClient)
        {
            _repository = repository;
            this._commandDataClient = _commandDataClient;
            _messageBusClient = messageBusClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<DataCatalog>> GetDataCatalog()
        {
            Console.WriteLine("--> Getting Platforms....");

            var platformItems = _repository.Get();

            return Ok(platformItems);
        }

        [HttpGet("{id}", Name = "GetPlatformById")]
        public ActionResult<DataCatalog> GetDataCatalogById(string id)
        {
            var platformItem = _repository.GetById(id);
            if (platformItem is not null)
            {
                return Ok(platformItem);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<DataCatalog>> CreateDataCatalog(DataCatalog catalog)
        {
           var result =  _repository.Create(catalog);
           
           // Sending Sync Message..
           try
           {
               await _commandDataClient.SendDataCatalogToCommand(catalog);

           }
           catch (Exception e)
           {
               Console.WriteLine(e);
               Console.WriteLine("--> Could Not send sync msg to command ");
               
           }
           
           // Sending Async Message..

           try
           {
               _messageBusClient.PublishNewDataCatalog(catalog);
               

           }
           catch (Exception e)
           {
               Console.WriteLine(e);
               Console.WriteLine("--> Could Not send async msg using Rabbit MQ ");
           }
           return CreatedAtAction(nameof(GetDataCatalogById), new { id = result.Id }, result);
        }
    }
}