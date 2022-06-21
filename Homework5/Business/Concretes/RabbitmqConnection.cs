using Homework5.Business.Abstracts;
using RabbitMQ.Client;

namespace  Homework5.Business.Concretes
{
    public class RabbitmqConnection : IRabbitmqConnection
    {
        public IConnection GetRabbitMqConnection()
        {
            var connectionFactory = new ConnectionFactory()
            {
                HostName = "localhost",
                VirtualHost = "/",
                Port = 5672,
                UserName = "guest",
                Password = "guest"

            }.CreateConnection();

            return connectionFactory;
        }
    }
}
