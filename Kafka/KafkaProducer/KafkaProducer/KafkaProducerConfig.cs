using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaProducer
{
    public class KafkaProducerConfig
    {
        public string BootstrapServers { get; set; }
        public string ClientId { get; set; }
        public string SecurityProtocol { get; set; }
        public string SaslMechanism { get; set; }
        public string SaslUsername { get; set; }
        public string SaslPassword { get; set; }
        public string TopicSubscribed { get; set; }
    }
}
