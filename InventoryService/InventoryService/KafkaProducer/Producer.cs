using Confluent.Kafka;

namespace InventoryService.KafkaProducer
{
    public class Producer : IProducer
    {
        private readonly IProducer<string, string> _producer;
        public Producer(IConfiguration configuration)
        {
            var config = new ProducerConfig();

            configuration.GetSection("Kafka").Bind(config);

            _producer = new ProducerBuilder<string, string>(config)
                .Build();
        }

        public async Task ProduceMessage(string key, string topic, string message)
        {
            try
            {
                var deliveryResult = await _producer.ProduceAsync(topic, new Message<string, string>
                {
                    Key = key,
                    Value = message
                });

                Console.WriteLine($"Produced message to: {deliveryResult.TopicPartitionOffset}");
            }
            catch (ProduceException<string, string> ex)
            {
                Console.WriteLine($"Error producing message: {ex.Error.Reason}");
            }
        }
    }
}
