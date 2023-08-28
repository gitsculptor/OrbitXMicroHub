using DataCatalogService.Models;
using Microsoft.EntityFrameworkCore;

namespace DataCatalogService.Data;

public class AppDbContext :DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
    {
            
    }

    public DbSet<DataCatalog> Platforms { get; set; }
}