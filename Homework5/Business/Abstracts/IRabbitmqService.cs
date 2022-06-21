using Microsoft.AspNetCore.Identity;

namespace Homework5.Business.Abstracts
{
    public interface IRabbitmqService
    {
        void Publish(IdentityUser user, string exchangeType, string exchangeName, string queueName, string routeKey);
    }
}
