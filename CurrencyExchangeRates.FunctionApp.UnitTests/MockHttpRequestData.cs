using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using Moq;
using System.Text.Json;

namespace CurrencyExchangeRates.FunctionApp.UnitTests
{
    public class MockHttpRequestData
    {
        public static HttpRequestData Create()
        {
            return Create("");
        }

        public static HttpRequestData Create<T>(T requestData) where T : class
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddFunctionsWorkerDefaults();

            var serializedData = JsonSerializer.Serialize(requestData);
            var bodyDataStream = new MemoryStream(Encoding.UTF8.GetBytes(serializedData));

            var context = new Mock<FunctionContext>();
            context.SetupProperty(context => context.InstanceServices, serviceCollection.BuildServiceProvider());

            var request = new Mock<HttpRequestData>(context.Object);
            request.Setup(r => r.Body).Returns(bodyDataStream);

            return request.Object;
        }
    }
}
