using Confluent.Kafka;
using System;

namespace Akka.Kafka.Connector.Producer
{
    internal class Program
    {
       
        private static void Main(string[] args)
        {
            var ping =Util.PingHost("kafka", true);

            string topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC_NAME");

            var config = new ProducerConfig { BootstrapServers = Environment.GetEnvironmentVariable("KAFKA_BROKER_LIST") };

            TestProducerWrapper<Null, string> producer = new TestProducerWrapper<Null, string>(config);
            producer.Produce<Null, string>(null, "Hello world!", topic);
        }
    }
}
