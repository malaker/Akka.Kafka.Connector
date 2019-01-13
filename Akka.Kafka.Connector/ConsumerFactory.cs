using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Text;

namespace Akka.Kafka.Connector
{
    public class ConsumerFactory<K, V> : IConsumerFactory<K, V>
    {
        private Dictionary<string, string> config;
        private Deserializer<K> kdeserializer;
        private Deserializer<V> vdeserializer;

        public ConsumerFactory(Dictionary<string,string> config,Confluent.Kafka.Deserializer<K> kdeserializer, Confluent.Kafka.Deserializer<V> vdeserializer)
        {
            this.config = config;
            this.kdeserializer = kdeserializer;
            this.vdeserializer = vdeserializer;
        }
        public IConsumerWrapper<K, V> Create()
        {
            Consumer<K,V> consumer= new Consumer<K, V>(config, kdeserializer, vdeserializer);
            return new ConsumerWrapper<K, V>(consumer);
        }
    }
}
