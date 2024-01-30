using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaConsumer
{
    public class KafkaConsumerConfig
    {
        public string GroupId { get; set; }
        public string BootstrapServers { get; set; }
        public string ClientId { get; set; }
        public string SaslUsername { get; set; }
        public string SaslPassword { get; set; }
        public string TopicSubscribed { get; set; }
    }
}
