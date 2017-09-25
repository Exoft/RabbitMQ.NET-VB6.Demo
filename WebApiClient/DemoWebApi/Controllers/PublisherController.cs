using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System;
using System.Collections.Concurrent;
using System.Text;
using System.Timers;
using RabbitMQ.Client.Events;

namespace DemoWebApi.Controllers
{
    [Route("api/[controller]")]
    public class PublisherController : Controller
    {

        [HttpPost]
        public IActionResult Post(string message,string queueName = "hello", string hostName =  "13.65.87.69")
        {
            //var factory = new ConnectionFactory() { HostName =  hostName};
            //using (var connection = factory.CreateConnection())
            //using (var channel = connection.CreateModel())
            //{
            //    channel.QueueDeclare(queue: queueName,
            //                         durable: false,
            //                         exclusive: false,
            //                         autoDelete: false,
            //                         arguments: null);

            //    var body = Encoding.UTF8.GetBytes(message);

            //    channel.BasicPublish(exchange: "",
            //        routingKey: "hello",
            //        basicProperties: null,
            //        body: body);
            //    Console.WriteLine(" [x] Sent {0}", message);

            //}

            var rpcClient = new RpcClient();


            var tm = new Timer(10000);
            tm.Elapsed += (sender, args) =>
            {
                rpcClient.Timeout();
            };
            tm.AutoReset = false;
            tm.Start();

            var response = rpcClient.Call(message);
            rpcClient.Close();
            tm.Stop();
            return Ok(response);
        }
    }


    public class RpcClient
    {
        private readonly IConnection connection;
        private readonly IModel channel;
        private readonly string replyQueueName;
        private readonly EventingBasicConsumer consumer;
        private readonly BlockingCollection<string> respQueue = new BlockingCollection<string>();
        private readonly IBasicProperties props;

        public RpcClient()
        {
            var factory = new ConnectionFactory() { HostName = "13.65.87.69" };

            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            channel.QueueDeclare(queue: "replyQUEUE",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            //replyQueueName = channel.QueueDeclare().QueueName;
            replyQueueName = "replyQUEUE";
            consumer = new EventingBasicConsumer(channel);

            props = channel.CreateBasicProperties();
            var correlationId = Guid.NewGuid().ToString();
            props.CorrelationId = correlationId;
            props.ReplyTo = replyQueueName;
            props.Expiration = "10000";

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var response = Encoding.UTF8.GetString(body);
                if (ea.BasicProperties.CorrelationId == correlationId)
                {
                    respQueue.Add(response);
                }
            };
        }

        public void Timeout()
        {
            respQueue.Add("Timeout");
        }

        public string Call(string message)
        {
            var messageBytes = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(
                exchange: "",
                routingKey: "hello",
                basicProperties: props,
                body: messageBytes);

            channel.BasicConsume(
                consumer: consumer,
                queue: replyQueueName,
                autoAck: true);

            return respQueue.Take(); ;
        }

        public void Close()
        {
            connection.Close();
        }
    }
}
