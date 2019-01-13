using System;

namespace Akka.Kafka.Connector.Shared
{
    public class Message<K,V>
    {
        public Message(K key, V value, string topic, int partition,long offset)
        {
            this.Key = key;
            this.Value = value;
            this.Topic = topic;
            this.Partition = partition;
            this.Offset = offset;
        }

        public K Key { get; }

        public V Value { get; }

        public string Topic { get; }

        public int Partition { get; }

        public long Offset { get; }
    }
}
