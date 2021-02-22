using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PeanutButterConsumerApi.WorkerServices
{
    public class KafkaWorkerService : BackgroundService
    {
        private readonly string topic;
        private readonly IConsumer<string, string> _kafkaConsumer;

        public KafkaWorkerService(IConfiguration config)
        {
            var consumerConfig = new ConsumerConfig { BootstrapServers = "localhost:9092", GroupId = "1" };
            this.topic = "NIVASH";
            this._kafkaConsumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            new Thread(() => StartConsumer(stoppingToken)).Start();
            return Task.CompletedTask;
        }

        private void StartConsumer(CancellationToken cancellationToken)
        {
            _kafkaConsumer.Subscribe(this.topic);

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var cr = this._kafkaConsumer.Consume(cancellationToken);

                    // Handle message...
                    Console.WriteLine($"{cr.Message.Key}: {cr.Message.Value}");
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (ConsumeException e)
                {
                    Console.WriteLine($"Consume error: {e.Error.Reason}");

                    if (e.Error.IsFatal)
                    {
                        break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Unexpected error: {e}");
                    break;
                }
            }
        }
    }
}
