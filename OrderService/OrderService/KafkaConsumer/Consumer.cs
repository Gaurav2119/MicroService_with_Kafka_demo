using Confluent.Kafka;
using Newtonsoft.Json;
using OrderService.DataAccess.Interface;
using OrderService.Models;

namespace OrderService.KafkaConsumer
{
    public class Consumer : IConsumer
    {
        private readonly IConsumer<string, string> _consumer;
        private readonly ILogger<Consumer> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public Consumer(IConfiguration configuration, ILogger<Consumer> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;

            var config = new ConsumerConfig();

            configuration.GetSection("Kafka").Bind(config);

            _consumer = new ConsumerBuilder<string, string>(config).Build();
            _scopeFactory = scopeFactory;

        }

        public void ConsumeMessage(string topic, CancellationToken cts, IOrder orderAccess)
        {
            _consumer.Subscribe(topic);

            cts.Register(() =>
            {
                _logger.LogInformation("Kafka Consumer is stopping.");
                _consumer.Close();
            });

            try
            {
                while (!cts.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = _consumer.Consume(cts);

                        if (consumeResult.IsPartitionEOF)
                        {
                            _logger.LogInformation($"Reached end of partition: {consumeResult.TopicPartition}");
                            continue;
                        }

                        _logger.LogInformation($"Consumed message '{consumeResult.Message.Value}' at: '{consumeResult.TopicPartitionOffset}'.");

                        // Deserialize the message
                        dynamic? kafkaKey = JsonConvert.DeserializeObject(consumeResult.Message.Key);
                        dynamic? kafkaMessage = JsonConvert.DeserializeObject(consumeResult.Message.Value);

                        // Extract key and message and create an Order object
                        Order _order = new Order
                        {
                            productId = Convert.ToInt32(kafkaKey), // Assuming the key is an integer
                            quantity = kafkaMessage?.quantity
                        };

                        orderAccess.addOrder(_order);
                    }

                    catch (ConsumeException ex)
                    {
                        Console.WriteLine($"Error occurred: {ex.Error.Reason}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
            }
        }
    }
}
