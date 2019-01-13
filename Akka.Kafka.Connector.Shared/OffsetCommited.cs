namespace Akka.Kafka.Connector.Shared
{
    public class OffsetCommited
    {
        public OffsetCommited(CommitOffset commitOffset)
        {
            this.Topic = commitOffset.Topic;
            this.Partition = commitOffset.Partition;
            this.Offset = commitOffset.Offset;
        }

        public string Topic { get; }

        public int Partition { get; }

        public long Offset { get; }
    }
}
