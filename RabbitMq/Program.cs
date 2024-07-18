using RabbitMQ.Client;
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

Console.WriteLine("Digite o nome do aluno e aperte <ENTER>");

while (true)
{
    string nomeAluno = Console.ReadLine();

    if (nomeAluno == "")
        break;

    var novoAluno = new Aluno() { Id = Guid.NewGuid(), Nome = nomeAluno };
    string aluno = JsonSerializer.Serialize(novoAluno);

    var corpo = Encoding.UTF8.GetBytes(aluno);

    channel.BasicPublish(exchange: string.Empty,
                         routingKey: "hello",
                         basicProperties: null,
                         body: corpo);

    Console.WriteLine($"Enviado {aluno}");
}

class Aluno
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
}