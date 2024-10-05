using System.Text;
using HealthMed.Application.Common.Services;
using RabbitMQ.Client;

namespace HealthMed.Infrastructure.RabbitMQ;

public class MessagePublisherService(IConnection connection) : IMessagePublisherService
{
    public void PublishMessage
    (
        string message,
        string queueName,
        bool durable = false
    )
    {
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: queueName,
                             durable: durable,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: "",
                             routingKey: queueName,
                             basicProperties: null,
                             body: body);
    }
}
