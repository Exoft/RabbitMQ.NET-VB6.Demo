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
    public interface IRabbitClient:IDisposable
    {
        [DispId(1)]
        string Initialize(string hostName, string queueName);

        [DispId(2)]
        void MessageReceived(MessageEventArgs e);
    }

    [Guid("8AF98AD8-30D0-4161-AECC-92D495F17745")]
    [ComSourceInterfaces(typeof(IRabbitClient))]
    [ComVisible(true)]
    public class RabbitClient : IDisposable
    {
        private IModel _channel;
        private IConnection _connection;

        public event MessageReceivedHandler MessageReceived;

        public string Initialize(string hostName, string queueName)
        {
            var result = "OK";
            try
            {
                var factory = new ConnectionFactory {HostName = hostName};
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.QueueDeclare(queueName,
                    true,
                    false,
                    false,
                    null);

                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    MessageReceived?.Invoke(new MessageEventArgs {Message = message});
                };
                _channel.BasicConsume(queueName,
                    true,
                    consumer);
            }
            catch (Exception exception)
            {
                result = exception.Message;
            }

            return result;
        }

        #region IDisposable Support

        private bool _disposedValue; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                _connection?.Dispose();
                _channel?.Dispose();
                
                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                _disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        ~RabbitClient()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion
    }

    [ClassInterface(ClassInterfaceType.AutoDual)]
    [Guid("EDD6DD7F-730F-4E97-A040-11A2C59D29C8")]
    [ComVisible(true)]
    public class MessageEventArgs : EventArgs
    {
        public string Message { set; get; }
    }
}