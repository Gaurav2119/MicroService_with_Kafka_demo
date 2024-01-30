using OrderService.DataAccess.Interface;

namespace KafkaConsumer
{
    public interface IConsumer
    {
        void ConsumeMessage(CancellationToken cts, IOrder order);
        //public void RunInBackground();
    }
}
