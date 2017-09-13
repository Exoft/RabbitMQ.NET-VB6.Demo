using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using VB6ComLib;

namespace TestApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var client = new RabbitClient();
            client.MessageReceived += ClientOnMessageReceived;
            client.Initialize("localhost", "hello");





            Console.ReadLine();
        }

        private static void ClientOnMessageReceived(MessageEventArgs messageEventArgs)
        {
            Console.WriteLine(messageEventArgs.Message);
        }
    }
}