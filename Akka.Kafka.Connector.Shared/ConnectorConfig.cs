using System;
using System.Collections.Generic;
using System.Text;

namespace Akka.Kafka.Connector.Shared
{
    public class ConnectorConfig
    {
        public PersistanceConnectorConfig PersistanceConnectorConfig { get; set; }
        public TimeSpan PollPeriod { get; set; }
        public Dictionary<string,string> Settings { get; set; }
        public IEnumerable<string> Topic { get; set; }
    }

    public class PersistanceConnectorConfig
    {
        public string PersistanceId { get; set; }
    }
}
