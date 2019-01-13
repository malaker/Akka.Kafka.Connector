using System;
using Confluent.Kafka;
using Moq;

namespace Akka.Kafka.Connector.Tests
{
    public class BaseConsumerActorFixture : Akka.TestKit.Xunit.TestKit
    {

        public Tuple<Mock<IConsumerFactory<K, V>>, Mock<IConsumerWrapper<K, V>>> SetupMocks<K, V>(Confluent.Kafka.ConsumeResult<K, V> consumerResult)
        {
            var factory = new Mock<IConsumerFactory<K, V>>();
            var consumer = new Mock<IConsumerWrapper<K, V>>();

            consumer.Setup(m => m.Consume()).Returns(consumerResult);

            factory.Setup(m => m.Create()).Returns(consumer.Object);

            return new Tuple<Mock<IConsumerFactory<K, V>>, Mock<IConsumerWrapper<K, V>>>(factory, consumer);
        }

        public Confluent.Kafka.ConsumeResult<K, V> PrepareResult<K, V>(K key, V value)
        {
            Confluent.Kafka.Message<K, V> kafkamessage = new Confluent.Kafka.Message<K, V>() { Value = value, Timestamp = new Timestamp() };
            var consumerResult = new ConsumeResult<K, V>() { Message = kafkamessage, Topic = "Test", Partition = 1, Offset = 1 };
            return consumerResult;
        }
    }
}
