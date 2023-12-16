using System.Text;
using System.Text.Json;
using DataCatalogService.Models;
using Microsoft.EntityFrameworkCore.Metadata;
using RabbitMQ.Client;
using IModel = RabbitMQ.Client.IModel;

namespace DataCatalogService.AsyncDataServices;

public class MessageBusClient:IMessageBusClient
{
    private readonly IConfiguration _configuration;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public MessageBusClient(IConfiguration configuration)
    {
        _configuration = configuration;
        var factory = new ConnectionFactory()
        {
            HostName = _configuration["RabbitMQHost"],
            Port = Convert.ToInt32(_configuration["RabbitMQPort"])
        };

        try
        {
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare("trigger",ExchangeType.Fanout);

            _connection.ConnectionShutdown += RabbitMq_ConnectionShutdown;
            
            Console.WriteLine("Connected to message bus");
            


        }
        catch (Exception e)
        {
            Console.WriteLine($"Could not connect to message bus,,{e.Message}");
            throw;
        }

    }

    private void RabbitMq_ConnectionShutdown(object sender, ShutdownEventArgs e)
    {
        Console.WriteLine($"RabbitMQ connection shutdown. Reason: {e.ReplyText}");
       
    }

    

    public void PublishNewDataCatalog(DataCatalog data)
    {
        var message = JsonSerializer.Serialize(data);
        if (_connection.IsOpen)
        {
            Console.WriteLine("rabbit mq connection open , sending message");
            sendMessage(message);
        }
        else
        {
            Console.WriteLine("connection not open , not sending");
        }
        
    }

    private void sendMessage(string message)
    {
        try
        {
            var body = Encoding.UTF8.GetBytes(message);
            string exchange = "trigger";
            string routingKey = "";
            _channel.BasicPublish(exchange, routingKey, null, body);
            Console.WriteLine("Message published successfully");
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error publishing message: {ex.Message}");
        }
    }

    private void dispose()
    {
        Console.WriteLine("message bus disposed");
        if (_channel.IsOpen)
        {
            _channel.Close();
            _connection.Close();
        }
        
            
        
    }
}