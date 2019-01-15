using System;
using Xunit;

namespace Akka.Kafka.Connector.IntegrationTests
{
    public class BaseAkkaKafkaIntegrationTest : Akka.TestKit.Xunit.TestKit
    {
        protected string topic;
        protected string brokerlist;

        public BaseAkkaKafkaIntegrationTest()
        {
            var kafkaPingable = Akka.Kafka.Connector.Producer.Util.PingHost("kafka");

            Assert.True(kafkaPingable);


            this.topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC_NAME");

            if (topic == null)
            {
                topic = "TestTopic";
            }

            this.brokerlist = Environment.GetEnvironmentVariable("KAFKA_BROKER_LIST");
            if (brokerlist == null)
            {
                brokerlist = "localhost:9092";
            }

        }
    }
}
