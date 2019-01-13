using Akka.Kafka.Connector.Shared;
using Confluent.Kafka;
using System;
using Xunit;

namespace Akka.Kafka.Connector.Tests
{
    public class ConsumerEventPublisherActorFixture : BaseConsumerActorFixture
    {
        public ConnectorConfig Config { get; private set; }

        public ConsumerEventPublisherActorFixture()
        {
            this.Config = new ConnectorConfig();       
        }

        [Fact]
        public void Conusmer_should_publish_message_to_event_stream()
        {
            Config.PollPeriod = TimeSpan.FromSeconds(1);

            var consumeResult = PrepareResult<Ignore, string>(null, "Hello World!");

            var mocks = SetupMocks(consumeResult);

            var probe = this.CreateTestProbe();

            Sys.EventStream.Subscribe(probe, typeof(Shared.Message<Ignore,string>));

            var subject = this.Sys.ActorOf(Actor.Props.Create<ConsumerEventPublisherActor<Ignore, string>>(Config, mocks.Item1.Object));

            var message = probe.ExpectMsg<Shared.Message<Ignore, string>>();

            Assert.Equal(consumeResult.Value, message.Value);
            Assert.Equal(consumeResult.Topic, message.Topic);
            Assert.Equal(consumeResult.Partition.Value, message.Partition);
            Assert.Equal(consumeResult.Offset.Value, message.Offset);
        }
    }
}
