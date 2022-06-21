using System.Text;
using System.Text.Json;
using Homework5.Business.Abstracts;
using Microsoft.AspNetCore.Identity;
using RabbitMQ.Client;

namespace Homework5.Business.Concretes;

//Rabbitmq kuyruk ekleme metodu
public class RabbitmqManager : IRabbitmqService
{
    private readonly IRabbitmqConnection _connection;
    public RabbitmqManager(IRabbitmqConnection connection) => _connection = connection;

    public void Publish(IdentityUser user, string exchangeType, string exchangeName, string queueName, string routeKey)
    {

        using var connection = _connection.GetRabbitMqConnection();
        

        using var channel = connection.CreateModel();

        channel.ExchangeDeclare(exchangeName, exchangeType, false, false);

        channel.QueueDeclare(queueName, false, false, false);

        channel.QueueBind(queueName, exchangeName, routeKey);

        var message = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(user));

        channel.BasicPublish(exchangeName, routeKey, null, message);
    }
}
