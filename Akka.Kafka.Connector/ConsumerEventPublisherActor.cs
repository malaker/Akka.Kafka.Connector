using Akka.Actor;
using Akka.Kafka.Connector.Shared;

namespace Akka.Kafka.Connector
{
    public class ConsumerEventPublisherActor<K, V> : ReceiveActor
    {
        private IActorRef consumer;

        public ConsumerEventPublisherActor(ConnectorConfig config, IConsumerFactory<K,V> consumerFactory)
        {
            this.consumer = Context.ActorOf(Props.Create<ConsumerActor<K, V>>(config, consumerFactory, Self));
            Receive<Shared.Message<K, V>>(m => Context.System.EventStream.Publish(m));
            Receive<CommitOffset>(m => consumer.Tell(m, Self));
            Receive<PoisonPill>(m => consumer.Tell(m));
        }
    }
}
