using Akka.Actor;
using Akka.Kafka.Connector.Shared;
using Confluent.Kafka;
using System;
using Xunit;

namespace Akka.Kafka.Connector.Tests
{
    public class ConsumerAtLeastOneDeliveryActorFixture : BaseConsumerActorFixture
    {
        public ConsumerAtLeastOneDeliveryActorFixture()
        {
            this.Config = new ConnectorConfig() { PersistanceConnectorConfig=new PersistanceConnectorConfig() { PersistanceId="test-1"} };
        }

        public ConnectorConfig Config { get; }

        [Fact]
        public void Consumer_Should_Confirmed_message()
        {

            Config.PollPeriod = TimeSpan.FromSeconds(1);

            var consumeResult = PrepareResult<Ignore, string>(null, "Hello World!");

            var mocks = SetupMocks(consumeResult);

            var probe = this.CreateTestProbe();

            Sys.EventStream.Subscribe(probe, typeof(MessageConfirmed));

            ActorSelection selection = Sys.ActorSelection(probe.TestActor.Path);

            var subject = this.Sys.ActorOf(Actor.Props.Create<ConsumerAtLeastOneDelivery<Ignore, string>>(Config, mocks.Item1.Object, selection));


            var message = probe.ExpectMsg<MessageToConfirm<Ignore, string>>();

            subject.Tell(new MessageConfirmed(message.Id, consumeResult.Topic, consumeResult.Partition.Value, consumeResult.Offset.Value), probe);

            var confirmedMessage = probe.ExpectMsg<MessageConfirmed>();

            Assert.Equal(consumeResult.Topic, confirmedMessage.Topic);
            Assert.Equal(consumeResult.Partition.Value, confirmedMessage.Partition);
            Assert.Equal(consumeResult.Offset.Value, confirmedMessage.Offset);


        }
    }
}
