using System;
using Xunit;
using Confluent.Kafka;
using Akka.Kafka.Connector.Shared;

namespace Akka.Kafka.Connector.Tests
{
    public class ConsumerActorFixture : BaseConsumerActorFixture
    {
        public ConnectorConfig Config { get; private set; }

        public ConsumerActorFixture()
        {
            this.Config = new ConnectorConfig();
        }

        [Fact]
        public void Consumer_should_consume_message()
        {
            Config.PollPeriod = TimeSpan.FromSeconds(1);

            var consumeResult = PrepareResult<Ignore, string>(null, "Hello World!");

            var mocks = SetupMocks(consumeResult);

            var probe = this.CreateTestProbe();

            var subject = this.Sys.ActorOf(Actor.Props.Create<ConsumerActor<Ignore, string>>(Config, mocks.Item1.Object, probe));

            var message = probe.ExpectMsg<Shared.Message<Ignore, string>>();

            Assert.Equal(consumeResult.Value, message.Value);
            Assert.Equal(consumeResult.Topic, message.Topic);
            Assert.Equal(consumeResult.Partition.Value, message.Partition);
            Assert.Equal(consumeResult.Offset.Value, message.Offset);
        }

        [Fact]
        public void Consumer_should_commit_message()
        {
            Config.PollPeriod = TimeSpan.FromSeconds(1);

            Confluent.Kafka.Message<Ignore, string> kafkamessage = new Confluent.Kafka.Message<Ignore, string>() { Value = "Hello world!", Timestamp = new Timestamp() };

            var consumerResult = new ConsumeResult<Ignore, string>() { Message = kafkamessage, Topic = "Test", Partition = 1, Offset = 1 };

            var consumeResult = PrepareResult<Ignore, string>(null, "Hello World!");

            var mocks = SetupMocks(consumeResult);

            var probe = this.CreateTestProbe();

            Sys.EventStream.Subscribe(probe, typeof(OffsetCommited));

            var subject = this.Sys.ActorOf(Actor.Props.Create<ConsumerActor<Ignore, string>>(Config, mocks.Item1.Object, probe));

            var message = probe.ExpectMsg<Shared.Message<Ignore, string>>();

            var messageToCommit = new CommitOffset(message.Topic, message.Partition, message.Offset);


            subject.Tell(messageToCommit, probe);

            var offsetCommited = probe.ExpectMsg<OffsetCommited>();

            Assert.Equal(messageToCommit.Topic, offsetCommited.Topic);
            Assert.Equal(messageToCommit.Partition, offsetCommited.Partition);
            Assert.Equal(messageToCommit.Offset, offsetCommited.Offset);
        }
    }
}
