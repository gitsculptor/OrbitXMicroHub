using CommandService.Models;
using Microsoft.EntityFrameworkCore;

namespace CommandService.Data;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
    {
            
    }

    public DbSet<Command> Platforms { get; set; }
    
}