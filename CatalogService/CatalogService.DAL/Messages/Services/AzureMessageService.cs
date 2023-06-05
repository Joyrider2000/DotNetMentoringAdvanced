using Azure.Messaging.ServiceBus;
using CatalogService.BLL.Application.Services.Message;
using CatalogService.DAL.Configuration.Options;
using System.Text.Json;

namespace CatalogService.DAL.Messages.Services
{
    public class AzureMessageService : IMessageService
    {
        private AppOptions _appOptions { get; }

        public AzureMessageService(AppOptions appOptions)
        {
            _appOptions = appOptions;
        }

        public async Task Publish(object entity)
        {
            var clientOptions = new ServiceBusClientOptions
            {
                TransportType = ServiceBusTransportType.AmqpWebSockets
            };

            ServiceBusClient client = new ServiceBusClient(_appOptions.AzureConnectionString, clientOptions);
            ServiceBusSender sender = client.CreateSender(_appOptions.AzureQueueName);
            string msg = JsonSerializer.Serialize(entity);
            ServiceBusMessage message = new ServiceBusMessage(msg);
            await sender.SendMessageAsync(message);
        }
    }
}
