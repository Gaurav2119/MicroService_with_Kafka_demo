using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrderService.DataAccess.Interface;

namespace KafkaConsumer
{
    public class KafkaBackground : BackgroundService
    {
        private readonly ILogger<KafkaBackground> _logger;
        private readonly IConsumer _consumer;
        private readonly IServiceScopeFactory _scopeFactory;
        public KafkaBackground(ILogger<KafkaBackground> logger, IServiceScopeFactory serviceScopeFactory, IConsumer consumer)
        {
            _logger = logger;
            _consumer = consumer;
            _scopeFactory = serviceScopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Yield();
            _logger.LogInformation("Kafka Consumer is starting");

            using (var scope = _scopeFactory.CreateScope())
            {
                var orderDataAccess = scope.ServiceProvider.GetRequiredService<IOrder>();
                _consumer.ConsumeMessage(stoppingToken, orderDataAccess);
            }
            //await Task.Delay(1000);

        }
    }
}
