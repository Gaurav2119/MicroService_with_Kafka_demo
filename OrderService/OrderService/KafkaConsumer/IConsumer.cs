using OrderService.DataAccess.Interface;

namespace OrderService.KafkaConsumer
{
    public interface IConsumer
    {
        void ConsumeMessage(CancellationToken cts, IOrder order);
    }
}
