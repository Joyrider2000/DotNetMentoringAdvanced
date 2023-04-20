using Azure.Messaging.ServiceBus;
using CatalogService.BLL.Application.Services.Message;
using System.Text.Json;

namespace CatalogService.DAL.Messages.Services
{
    public class AzureMessageService : IMessageService
    {
        private string _connectionString { get; }
        private string _queueName { get; }

        public AzureMessageService(string connectionString, string queueName)
        {
            _connectionString = connectionString;
            _queueName = queueName;
        }

        public async Task Publish(object entity)
        {
            var clientOptions = new ServiceBusClientOptions
            {
                TransportType = ServiceBusTransportType.AmqpWebSockets
            };

            ServiceBusClient client = new ServiceBusClient(_connectionString, clientOptions);
            ServiceBusSender sender = client.CreateSender(_queueName);
            string msg = JsonSerializer.Serialize(entity);
            ServiceBusMessage message = new ServiceBusMessage(msg);
            await sender.SendMessageAsync(message);
        }
    }
}
