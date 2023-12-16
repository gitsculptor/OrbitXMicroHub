using DataCatalogService.AsyncDataServices;
using DataCatalogService.Data;
using DataCatalogService.Repository;
using DataCatalogService.SyncDataServices.Http;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<ICommandDataClient, CommandDataClient>();

builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();


// builder.Services.AddDbContext<AppDbContext>(opt =>
//     opt.UseInMemoryDatabase("InMem"));


// Register MongoDB repository
builder.Services.AddSingleton<IDataCatalogRepository>(sp =>
{
    var connectionString = "mongodb+srv://prakashcodes584:LJ6yuRJ4ukv1V1Ye@cluster0.srsgfwg.mongodb.net/?retryWrites=true&w=majority"; // Replace with your MongoDB connection string
    var databaseName = "microserviceProject"; // Replace with your MongoDB database name

    return new DataCatalogRepo(connectionString, databaseName);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
