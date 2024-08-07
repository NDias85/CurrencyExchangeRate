using System.Text;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace CurrencyExchangeRates.FunctionApp.UnitTests
{
    public class MockHttpRequest
    {
        public static HttpRequest Create(object body)
        {
            var json = JsonConvert.SerializeObject(body);

            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json));

            var context = new DefaultHttpContext();
            var request = context.Request;
            request.Body = memoryStream;
            request.ContentType = "application/json";

            return request;
        }
    }
}
