using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "hello",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

Console.WriteLine("Aguardando mensagens.");

var consumidor = new EventingBasicConsumer(channel);
consumidor.Received += (model, ea) =>
{
    var corpo = ea.Body.ToArray();
    var mensagem = Encoding.UTF8.GetString(corpo);

    var aluno = JsonSerializer.Deserialize<Aluno>(mensagem);
    Console.WriteLine($"Recebido: {mensagem}");
};

channel.BasicConsume(queue: "hello",
                     autoAck: true,
                     consumer: consumidor);

Console.WriteLine("Aperte <ENTER> para sair.");
Console.ReadLine();

class Aluno
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
}