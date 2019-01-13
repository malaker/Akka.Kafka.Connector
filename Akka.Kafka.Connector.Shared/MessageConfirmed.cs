namespace Akka.Kafka.Connector.Shared
{
    public class MessageConfirmed<K, V>
    {
        public MessageConfirmed(long id, Message<K, V> message)
        {
            this.Id=id;
            this.Message=message;
        }

        public long Id { get; }
        public Message<K, V> Message { get; }
    }
}
