using Confluent.Kafka;

namespace Akka.Kafka.Connector
{
    public interface IConsumerFactory<K, V>
    {
        IConsumerWrapper<K, V> Create();
    }
}
