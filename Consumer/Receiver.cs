using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Consumer
{
    public class Receiver
    {
        public static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            //var connection = factory.CreateConnection();
            using (var connection = factory.CreateConnection())
            {
                var channel = connection.CreateModel();

                channel.QueueDeclare(
                    queue: "BasicTest",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    Console.WriteLine($"Received message {message}...");
                };

                channel.BasicConsume(
                    queue: "BasicTest",
                    autoAck: true,
                    consumer: consumer);
            }

            Console.WriteLine("Press [enter] to exit the Consumer App");
            Console.ReadLine();
        }
    }
}
