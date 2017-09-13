using System;
using System.Runtime.InteropServices;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace VB6ComLib
{
    public delegate void MessageReceivedHandler(MessageEventArgs e);

    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    [Guid("2129D829-81C3-48E5-97F8-57AF64ABADF1")]
    public interface IRabbitClient
    {
        [DispId(1)]
        string Initialize(string hostName, string queueName);

        [DispId(2)]
        void MessageReceived(MessageEventArgs e);
    }

    [Guid("8AF98AD8-30D0-4161-AECC-92D495F17745")]
    [ComSourceInterfaces(typeof(IRabbitClient))]
    [ComVisible(true)]
    public class RabbitClient
    {
        public event MessageReceivedHandler MessageReceived;

        public string Initialize(string hostName, string queueName)
        {
            var result = "OK";
            try
            {
                var factory = new ConnectionFactory {HostName = "localhost"};
                var connection = factory.CreateConnection();
                var channel = connection.CreateModel();
                channel.QueueDeclare("hello",
                    true,
                    false,
                    false,
                    null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    MessageReceived.Invoke(new MessageEventArgs {Message = message});
                };
                channel.BasicConsume("hello",
                    true,
                    consumer);

                MessageReceived.Invoke(new MessageEventArgs { Message = "DEMO" });

            }
            catch (Exception exception)
            {
                result = exception.Message;
            }

            return result;
        }
    }

    [ClassInterface(ClassInterfaceType.AutoDual)]
    [Guid("EDD6DD7F-730F-4E97-A040-11A2C59D29C8")]
    [ComVisible(true)]
    public class MessageEventArgs : EventArgs
    {
        public string Message { set; get; }
    }
}