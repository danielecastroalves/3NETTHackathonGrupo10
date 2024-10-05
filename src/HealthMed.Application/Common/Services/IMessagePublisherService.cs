namespace HealthMed.Application.Common.Services
{
    public interface IMessagePublisherService
    {
        void PublishMessage(string message,
                            string queueName,
                            bool durable = false);
    }
}
