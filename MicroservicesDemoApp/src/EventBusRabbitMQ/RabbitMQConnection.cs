using EventBusRabbitMQ.Interfaces;
using RabbitMQ.Client;
using System;
using System.Threading;

namespace EventBusRabbitMQ
{
    public class RabbitMQConnection : IRabbitMQConnection
    {
        private readonly IConnectionFactory _connectionFactory;
        private IConnection _connection;
        private bool _disposed;

        public RabbitMQConnection(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;

            if (!IsConnected)
            {
                TryConnect();
            }
        }

        public bool IsConnected
        {
            get
            {
                return _connection != null && _connection.IsOpen && !_disposed;
            }
        }

        public bool TryConnect()
        {
            try
            {
                _connection = _connectionFactory.CreateConnection();
            }
            catch// (BrokerUnreachableException ex)
            {
                Thread.Sleep(2000);
                _connection = _connectionFactory.CreateConnection();
            }

            if (IsConnected)
                return true;
            else 
                return false;
        }

        public IModel CreateModel()
        {
            if (!IsConnected)
                throw new InvalidOperationException("Cannot connect to RabbitMQ Server.");

            return _connection.CreateModel();
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            try
            {
                _connection.Dispose();
            }
            catch
            {
                throw;
            }
        }
    }
}
