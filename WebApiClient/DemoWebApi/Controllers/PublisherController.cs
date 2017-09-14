using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System;
using System.Text;

namespace DemoWebApi.Controllers
{
    [Route("api/[controller]")]
    public class PublisherController : Controller
    {

        [HttpPost]
        public void Post([FromBody] string message = "Hello World !!!")
        {
            var factory = new ConnectionFactory() { HostName = "13.65.87.69" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello",
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                    routingKey: "hello",
                    basicProperties: null,
                    body: body);
                Console.WriteLine(" [x] Sent {0}", message);

            }

        }
    }
}
