using Akka.Kafka.Connector.Shared;
using Confluent.Kafka;
using System.Collections.Generic;

namespace Akka.Kafka.Connector
{
    public interface IConsumerWrapper<K, V>
    {
        bool IsDisposed { get; }

        ConsumeResult<K, V> Consume();

        void Close();

        void Subscribe(IEnumerable<string> topics);

        void Subscribe(string topic);

        void Commit(IEnumerable<CommitOffset> offsetsToCommit);

        void Commit(CommitOffset commitOffset);
    }
}
