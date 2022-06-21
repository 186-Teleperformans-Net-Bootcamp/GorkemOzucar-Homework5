using Hangfire;
using HangfireService.Services;
using Microsoft.AspNetCore.Identity;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace HangfireService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                var connectioFactory = new ConnectionFactory()
                {
                    HostName = "localhost",
                    VirtualHost = "/",
                    Port = 5672,
                    UserName = "guest",
                    Password = "guest"
                };

                var connection = connectioFactory.CreateConnection();

                var channel = connection.CreateModel();

                channel.QueueDeclare("direct.queuName", false, false, false);

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (sender, args) =>
                {
                    var message = JsonSerializer.Deserialize<IdentityUser>(Encoding.UTF8.GetString(args.Body.ToArray()));
                    foreach (PropertyInfo p in message.GetType().GetProperties())
                        Console.WriteLine(p.Name + " : " + p.GetValue(message));
                    Console.WriteLine();
                };

                channel.BasicConsume("direct.queuName", false, consumer);

            }
        }
    }
}