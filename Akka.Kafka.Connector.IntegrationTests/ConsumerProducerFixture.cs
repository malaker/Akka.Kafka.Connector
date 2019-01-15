using Akka.Kafka.Connector.Producer;
using Akka.Kafka.Connector.Shared;
using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Akka.Kafka.Connector.IntegrationTests
{
    public class ConsumerProducerFixture : BaseAkkaKafkaIntegrationTest
    {
        private ConnectorConfig connectorConfig;

        public ConsumerProducerFixture()
        {
            this.connectorConfig = new ConnectorConfig()
            {
                Topic = new List<string>() { topic },
                PollPeriod = TimeSpan.FromMilliseconds(10),
                Settings = new System.Collections.Generic.Dictionary<string, string>() {
                    { "bootstrap.servers",brokerlist },
                    { "client.id","test1" },
                {"group.id","test1g" },
                    { "auto.offset.reset","latest" }
                }
            };
        }

        [Fact]
        public async Task Should_consume_and_produce()
        {
           
            var probe = CreateTestProbe();
            var config = new ProducerConfig { BootstrapServers = brokerlist };
            var consumer = Sys.ActorOf(Actor.Props.Create<ConsumerActor<Null, string>>(connectorConfig, new ConsumerFactory<Null, string>(connectorConfig.Settings, null, Deserializers.UTF8), probe));
        
            TestProducerWrapper<Null, string> producer = new TestProducerWrapper<Null, string>(config);

            string rawMessage = "Hello world!";

            var deliveryReport = await producer.Produce<Null, string>(null, rawMessage, topic);

            var message = probe.ExpectMsg<Shared.Message<Null, string>>(TimeSpan.FromMinutes(1));

            Assert.Equal(rawMessage, message.Value);
            Assert.Equal(topic, message.Topic);
            Assert.Equal(deliveryReport.Offset.Value, message.Offset);
            Assert.Equal(deliveryReport.Partition.Value, message.Partition);
        }
    }
}
