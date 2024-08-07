namespace CurrencyExchangeRates.Core.Services
{
    public interface IServiceBusQueueSender
    {
        public Task SendAsync(string message);
    }
}
