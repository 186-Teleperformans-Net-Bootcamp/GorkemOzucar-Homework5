using RabbitMQ.Client;

namespace Homework5.Business.Abstracts
{
    public interface IRabbitmqConnection
    {
        IConnection GetRabbitMqConnection();
    }
}
