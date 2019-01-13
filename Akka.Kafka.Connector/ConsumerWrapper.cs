using System.Collections.Generic;
using System.Linq;
using Akka.Kafka.Connector.Shared;
using Confluent.Kafka;

namespace Akka.Kafka.Connector
{
    public class ConsumerWrapper<K, V> : IConsumerWrapper<K, V>
    {
        private Consumer<K, V> consumer;
        private bool isDisposed;
        public ConsumerWrapper(Consumer<K, V> consumer)
        {
            this.consumer = consumer;
        }

        public bool IsDisposed => isDisposed;

        public void Close()
        {
            consumer.Close();
            this.consumer.Dispose();
            isDisposed = true;
        }

        public void Commit(IEnumerable<CommitOffset> offsetsToCommit)
        {
            consumer.Commit(offsetsToCommit.Select(m => new Confluent.Kafka.TopicPartitionOffset(
                   m.Topic,
                   new Confluent.Kafka.Partition(m.Partition),
                   new Confluent.Kafka.Offset(m.Offset))));
        }

        public void Commit(CommitOffset commitOffset)
        {
            consumer.Commit(new List<Confluent.Kafka.TopicPartitionOffset>() {
                new Confluent.Kafka.TopicPartitionOffset(
                    commitOffset.Topic,
                    new Confluent.Kafka.Partition(commitOffset.Partition),
                    new Confluent.Kafka.Offset(commitOffset.Offset)) });
        }

        public ConsumeResult<K, V> Consume()
        {
            return consumer.Consume();
        }

        public void Subscribe(IEnumerable<string> topics)
        {
            this.consumer.Subscribe(topics);
        }

        public void Subscribe(string topic)
        {
            this.consumer.Subscribe(topic);
        }
    }
}
