namespace Akka.Kafka.Connector.Shared
{
    public class MessageToConfirm<K, V>
    {
        private long id;
        private Message<K, V> message;

       
        public MessageToConfirm(long id, Message<K, V> message)
        {
            this.Id = id;
            this.Message = message;
        }

        public long Id { get; }
        public Message<K, V> Message { get; }
    }
}
