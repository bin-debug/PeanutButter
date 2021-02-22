using Confluent.Kafka;
using PeanutButterCommon.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeanutButterProducerApi.Repository
{
    public class KafkaRespository : IServiceBus
    {
        public async Task<bool> PublishAsync(string topicName, string data)
        {
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            using (var p = new ProducerBuilder<string, string>(config).Build())
            {
                try
                {
                    var dr = await p.ProduceAsync(topicName, new Message<string, string> { Key = $"Key-{topicName}", Value = data });
                    return true;
                }
                catch (ProduceException<Null, string> e)
                {
                    Console.WriteLine($"Delivery failed: {e.Error.Reason}");
                    return false;
                }
            }
        }
    }
}
