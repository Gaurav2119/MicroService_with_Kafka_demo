namespace InventoryService.KafkaProducer
{
    public interface IProducer
    {
        Task ProduceMessage(string key, string topic, string message);
    }
}
