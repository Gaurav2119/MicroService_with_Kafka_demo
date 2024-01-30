using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaProducer
{
    public interface IProducer
    {
        Task ProduceMessage(Guid key, object message);
    }
}
