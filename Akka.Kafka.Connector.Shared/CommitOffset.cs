namespace Akka.Kafka.Connector.Shared
{
    public class CommitOffset
    {
        public CommitOffset(string topic, int partition, long offset)
        {
            this.Topic = topic;
            this.Partition = partition;
            this.Offset = offset;
        }

        public string Topic { get; }

        public int Partition { get; }

        public long Offset { get; }
    }
}
