using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "hello",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

Console.WriteLine(" [*] Esperando por mensagens");

var consumer = new EventingBasicConsumer(channel);
consumer.Received += Consumer_Received;

channel.BasicConsume(queue: "hello",
                     autoAck: true,
                     consumer: consumer);

Console.WriteLine(" Pressione [enter] para sair.");
Console.ReadLine();

void Consumer_Received(object? sender, BasicDeliverEventArgs e)
{
    var body = e.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);

    Console.WriteLine($"[x] Mensagem recebida {message}");
}

