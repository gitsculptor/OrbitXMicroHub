using DataCatalogService.Models;
using DataCatalogService.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DataCatalogService.Controllers
{
    [ApiController]
    [Route("api/data_catalog")]
    public class DataCatalogController : ControllerBase
    {
        private readonly IDataCatalogRepository _repository;

        public DataCatalogController(IDataCatalogRepository repository)
        {
            _repository = repository;
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
        public ActionResult<DataCatalog> CreateDataCatalog(DataCatalog catalog)
        {
           var result =  _repository.Create(catalog);
           return CreatedAtAction(nameof(GetDataCatalogById), new { id = result.Id }, result);
        }
    }
}