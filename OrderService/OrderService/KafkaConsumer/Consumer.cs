using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OrderService.DataAccess.Interface;
using OrderService.Models;

namespace OrderService.KafkaConsumer
{
    public class Consumer : IConsumer
    {
        private readonly IConsumer<string, string> _consumer;
        private readonly ILogger<Consumer> _logger;
        private readonly string _topic;

        public Consumer(IOptions<KafkaConsumerConfig> kafkaConsumerConfig, ILogger<Consumer> logger)
        {
            _logger = logger;

            var config = new ConsumerConfig
            {
                GroupId = kafkaConsumerConfig.Value.GroupId,
                BootstrapServers = kafkaConsumerConfig.Value.BootstrapServers,
                ClientId = kafkaConsumerConfig.Value.ClientId,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslMechanism = SaslMechanism.Plain,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                SaslUsername = kafkaConsumerConfig.Value.SaslUsername,
                SaslPassword = kafkaConsumerConfig.Value.SaslPassword
            };

            _topic = kafkaConsumerConfig.Value.TopicSubscribed;

            _consumer = new ConsumerBuilder<string, string>(config).Build();

        }

        public void ConsumeMessage(CancellationToken cts, IOrder orderAccess)
        {
            _consumer.Subscribe(_topic);

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
                            Id = new Guid(),
                            productId = new Guid(kafkaKey), // Assuming the key is an Guid
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
