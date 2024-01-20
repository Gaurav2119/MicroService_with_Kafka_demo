using OrderService.DataAccess.Interface;

namespace OrderService.KafkaConsumer
{
    public interface IConsumer
    {
        void ConsumeMessage(string topic, CancellationToken cts, IOrder order);
    }
}
