using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Text;
using Newtonsoft.Json;
using Consumer.RabbitMQ.Worker.Models;

namespace Consumer.RabbitMQ.Worker.Service
{
    public static class ConsumerService
    {
        public static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "root-queue",
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    var mensagem = JsonConvert.DeserializeObject<Mensagem>(message);

                    Console.WriteLine($"Código item: {mensagem.ItemId} | Nome: {mensagem.ItemNome}");

                    if (mensagem.ItemId % 2 == 0)
                    {
                        channel.BasicAck(ea.DeliveryTag, false);
                    }
                    else
                    {
                        channel.BasicNack(ea.DeliveryTag, false, false);
                    }
                };

                channel.BasicConsume(queue: "root-queue",
                                     autoAck: false,
                                     consumer: consumer);

    

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
