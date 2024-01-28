namespace InventoryService.KafkaProducer
{
    public interface IProducer
    {
        Task ProduceMessage(Guid key, object message);
    }
}
