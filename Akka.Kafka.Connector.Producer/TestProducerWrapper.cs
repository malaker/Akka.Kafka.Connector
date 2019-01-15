using Confluent.Kafka;
using System;
using System.Threading.Tasks;

namespace Akka.Kafka.Connector.Producer
{
    public class TestProducerWrapper<K, V>
    {
        private ProducerConfig config;

        public TestProducerWrapper(ProducerConfig config)
        {
            this.config = config;
        }

        public async Task<DeliveryReport<K,V>> Produce<K, V>(K key, V value, string topic)
        {
            using (var producer = new Producer<K, V>(config))
            {
                producer.OnError += Producer_OnError;
                producer.OnLog += Producer_OnLog;
                var result = await producer.ProduceAsync(topic, new Message<K, V>() { Value = value });

                producer.Flush(TimeSpan.FromSeconds(15));

                return result;
            }
        }

        private void Producer_OnLog(object sender, LogMessage e)
        {
            Console.WriteLine(e.Message);
        }

        private void Producer_OnError(object sender, ErrorEvent e)
        {
            Console.WriteLine(e.Reason);
        }
    }
}
