using Azure.Messaging.ServiceBus;
using CurrencyExchangeRates.Models.Settings;

namespace CurrencyExchangeRates.Core.Services
{
    public class ServiceBusQueueSender : IServiceBusQueueSender
    {
        private readonly string _connectionString;
        private readonly string _queueName;

        public ServiceBusQueueSender(ServiceBusQueueSettings settings)
        {
            _connectionString = settings.ConnectionString;
            _queueName = settings.QueueName;
        }

        public async Task SendAsync(string message)
        {
            await using var client = new ServiceBusClient(_connectionString);

            var sender = client.CreateSender(_queueName);

            var sbMessage = new ServiceBusMessage(message);

            await sender.SendMessageAsync(sbMessage);
        }
    }
}
