using Akka.Actor;
using Akka.Kafka.Connector.Shared;
using System;
using System.Collections.Generic;

namespace Akka.Kafka.Connector
{
    public class ConsumerActor<K,V> : ReceiveActor
    {
        protected ICanTell messageReceiver;
        protected ConnectorConfig config;
        protected IConsumerFactory<K, V> consumerFactory;
        protected IConsumerWrapper<K, V> consumer;
        protected ICancelable pollTask;
   

        public ConsumerActor(ConnectorConfig config, IConsumerFactory<K, V> consumerFactory, ICanTell messageReceiver)
        {
            this.messageReceiver = messageReceiver;
            this.config = config;
            this.consumerFactory = consumerFactory;
            Become(Consumable);
        }

        protected void Consumable()
        {
            consumer = consumerFactory.Create();
            PollMessage poll = new PollMessage();
            this.pollTask = Context.System.Scheduler.ScheduleTellRepeatedlyCancelable(TimeSpan.FromSeconds(0), config.PollPeriod, Self, poll, Self);
            Receive<PollMessage>(m => Consume());
            Receive<CommitOffset>(m => Commit(m));
            Receive<PoisonPill>(m => Close());
            consumer.Subscribe(config.Topic);
        }

        protected override void PostStop()
        {
            if (!consumer.IsDisposed)
            {
                Close();
            }

            base.PostStop();
        }

        protected void Close()
        {
            this.consumer.Close();

            if (!this.pollTask.IsCancellationRequested)
            {
                this.pollTask.Cancel();
            }
        }

        protected void Consume()
        {
            Confluent.Kafka.ConsumeResult<K,V> result =  consumer.Consume();

            if (result != null)
            {
                this.messageReceiver.Tell(new Shared.Message<K, V>(result.Key, result.Value, result.Topic, result.Partition.Value, result.Offset.Value),Self);
            }
        }

        protected void Commit(CommitOffset commitOffset)
        {
            this.consumer.Commit(commitOffset);
            Context.System.EventStream.Publish(new OffsetCommited(commitOffset));
        }
    }
}
