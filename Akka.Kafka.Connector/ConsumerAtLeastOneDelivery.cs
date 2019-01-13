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

            Command<Shared.Message<K, V>>(m => Handle(m));
            Command<CommitOffset>(m => consumer.Tell(m, Self));
            Command<MessageConfirmed<K, V>>(m => Handle(m));
            Command<PoisonPill>(m => consumer.Tell(m));
            Command<SaveSnapshotSuccess>(m => Handle(m));
            Recover<SnapshotOffer>(
        offer => offer.Snapshot is Akka.Persistence.AtLeastOnceDeliverySnapshot,
        offer =>
        {
            var snapshot = offer.Snapshot as Akka.Persistence.AtLeastOnceDeliverySnapshot;
            SetDeliverySnapshot(snapshot);
        });
        }

        protected void Handle(Shared.Message<K, V> message)
        {
            Persist(message, PersistHandler);
        }

        protected void Handle(MessageConfirmed<K, V> message)
        {
            ConfirmDelivery(message.Id);
            SaveSnapshot(GetDeliverySnapshot());
            Context.System.EventStream.Publish(message);
        }

        protected void Handle(SaveSnapshotSuccess message)
        {
            var seqNo = message.Metadata.SequenceNr;
            DeleteSnapshots(new SnapshotSelectionCriteria(seqNo,
            message.Metadata.Timestamp.AddMilliseconds(-1)));
        }

        protected void PersistHandler(Shared.Message<K, V> message)
        {
            Deliver(messageReceiver, id => new MessageToConfirm<K, V>(id, message));
            this.SaveSnapshot(this.GetDeliverySnapshot());
        }

        public override string PersistenceId => config.PersistanceConnectorConfig.PersistanceId;
    }
}
