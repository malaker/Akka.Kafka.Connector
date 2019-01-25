using Akka.Actor;
using Akka.Kafka.Connector.Shared;
using Akka.Persistence;

namespace Akka.Kafka.Connector
{
    public class ConsumerAtLeastOneDelivery<K, V> : AtLeastOnceDeliveryReceiveActor
    {
        protected ConnectorConfig config;
        protected ActorSelection messageReceiver;
        protected IActorRef consumer;

        public ConsumerAtLeastOneDelivery(ConnectorConfig config,IConsumerFactory<K,V> consumerFactory, ActorSelection messageReceiver)
        {
            this.config = config;
            this.messageReceiver = messageReceiver;
            this.consumer = Context.ActorOf(Props.Create<ConsumerActor<K, V>>(config, consumerFactory, Self));

            Command<Shared.Message<K, V>>(m => Persist(m,Handle));
            Command<CommitOffset>(m => consumer.Tell(m, Self));
            Command<MessageConfirmed>(m => Persist(m,Handle));
            Command<PoisonPill>(m => consumer.Tell(m));
            Recover<MessageConfirmed>(m => Handle(m));
            Recover<Shared.Message<K, V>>(m => Handle(m));
        }

        protected void Handle(MessageConfirmed message)
        {
            ConfirmDelivery(message.Id);
            Context.System.EventStream.Publish(message);
        }

        protected void Handle(Shared.Message<K, V> message)
        {
            Deliver(messageReceiver, id => new MessageToConfirm<K, V>(id, message));
        }

        public override string PersistenceId => config.PersistanceConnectorConfig.PersistanceId;
    }
}
