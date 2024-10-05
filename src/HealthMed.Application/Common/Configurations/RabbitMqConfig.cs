using System.Diagnostics.CodeAnalysis;

namespace HealthMed.Application.Common.Configurations
{
    [ExcludeFromCodeCoverage]
    public class RabbitMqConfig
    {
        public string ConnectionString { get; set; } = null!;
        public string NotificationQueue { get; set; } = null!;
    }
}
