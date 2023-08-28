using System.ComponentModel.DataAnnotations;

namespace DataCatalogService.Models;

public class DataCatalog
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Publisher { get; set; }

    [Required]
    public string Cost { get; set; }
}