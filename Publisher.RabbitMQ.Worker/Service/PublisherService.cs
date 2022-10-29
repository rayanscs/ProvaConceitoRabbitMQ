using Newtonsoft.Json;
using Publisher.RabbitMQ.Worker.Models;
using RabbitMQ.Client;
using System;
using System.Text;

namespace Publisher.RabbitMQ.Worker.Service
{
    public static class PublisherService
    {
        public static void Main()
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

                for (int i = 1; i <= 100; i++)
                {
                    var mensagem = new Mensagem 
                    { 
                        ItemId = i,
                        ItemNome = $"NomeItem{i}"
                    };

                    string message = JsonConvert.SerializeObject(mensagem);
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "local-exchange",
                                         routingKey: "root-queue",
                                         basicProperties: null,
                                         body: body);

                    Console.WriteLine(" [x] Sent {0}", message);
                }          
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
