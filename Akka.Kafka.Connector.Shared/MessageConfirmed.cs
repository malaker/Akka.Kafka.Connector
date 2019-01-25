namespace Akka.Kafka.Connector.Shared
{
    public class MessageConfirmed
    {
        public MessageConfirmed(long id,string topic,int partition,long offset)
        {
            this.Id=id;
            this.Topic = topic;
            this.Partition = partition;
            this.Offset = offset;
        }

        public long Id { get; }

        public string Topic { get;}

        public int Partition { get;}

        public long Offset { get; }
    }
}
