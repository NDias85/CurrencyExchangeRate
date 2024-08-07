using AutoMapper;
using CurrencyExchangeRates.Core.Mapping;
using CurrencyExchangeRates.Core.Services;
using CurrencyExchangeRates.Database.Repositories;
using CurrencyExchangeRates.Models.DTO;
using CurrencyExchangeRates.Models.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;

namespace CurrencyExchangeRates.Core.UnitTests
{
    public class CurrencyExchangeRateServiceTests : BaseTests
    {
        protected static IMapper Mapper => GetMapper();
        protected static HttpClient HttpClient => GetHttpClient();
        private readonly Mock<ICurrencyExchangeRateRepository> MockedCurrencyExchangeRateRepository = new();
        protected const string AlphaVantageResponse = "{\r\n    \"Realtime Currency Exchange Rate\": {\r\n        \"1. From_Currency Code\": \"EUR\",\r\n        \"2. From_Currency Name\": \"Euro\",\r\n        \"3. To_Currency Code\": \"GBP\",\r\n        \"4. To_Currency Name\": \"British Pound Sterling\",\r\n        \"5. Exchange Rate\": \"0.86110000\",\r\n        \"6. Last Refreshed\": \"2024-08-06 15:41:01\",\r\n        \"7. Time Zone\": \"UTC\",\r\n        \"8. Bid Price\": \"0.86106000\",\r\n        \"9. Ask Price\": \"0.86113000\"\r\n    }\r\n}";

        [Fact]
        public async Task GetCurrencyExchangeRateAsync_SuccessWithDbData()
        {
            // Arrange
            var currencyExchangeRate = new CurrencyExchangeRate { ExchangeRate = 1.2m };

            MockedCurrencyExchangeRateRepository
                .Setup(w => w.GetCurrencyExchangeRateAsync(It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(currencyExchangeRate);

            var service = new CurrencyExchangeRateService(Mock.Of<ILogger<CurrencyExchangeRateService>>(), MockedCurrencyExchangeRateRepository.Object, Mapper, HttpClient);

            // Act
            var result = await service.GetCurrencyExchangeRateAsync("USD", "EUR", CancellationToken.None);
            
            // Assert
            result.Should().NotBeNull();
            result!.ExchangeRate.Should().Be(currencyExchangeRate.ExchangeRate);
        }

        [Fact]
        public async Task GetCurrencyExchangeRateAsync_SuccessWithoutDbData()
        {
            // Arrange
            var entity = new CurrencyExchangeRate { ExchangeRate = 0.8611m };

            MockedCurrencyExchangeRateRepository
                .Setup(w => w.GetCurrencyExchangeRateAsync(It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(null as CurrencyExchangeRate);

            MockedCurrencyExchangeRateRepository
                .Setup(w => w.CreateCurrencyExchangeRateAsync(It.IsAny<CurrencyExchangeRate>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(entity);

            var service = new CurrencyExchangeRateService(Mock.Of<ILogger<CurrencyExchangeRateService>>(), MockedCurrencyExchangeRateRepository.Object, Mapper, HttpClient);

            // Act
            var result = await service.GetCurrencyExchangeRateAsync("USD", "EUR", CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result!.ExchangeRate.Should().Be(entity.ExchangeRate);
        }

        [Fact]
        public async Task CreateCurrencyExchangeRateAsync_Success()
        {
            // Arrange
            var entity = new CurrencyExchangeRate
            {
                FromCurrencyCode = "USD",
                ToCurrencyCode = "EUR",
                ExchangeRate = 12.3m,
                LastRefreshed = DateTime.UtcNow,
                AskPrice = 1.1m,
                BidPrice = 1.2m
            };

            MockedCurrencyExchangeRateRepository
                .Setup(w => w.CreateCurrencyExchangeRateAsync(It.IsAny<CurrencyExchangeRate>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(entity);

            var service = new CurrencyExchangeRateService(Mock.Of<ILogger<CurrencyExchangeRateService>>(), MockedCurrencyExchangeRateRepository.Object, Mapper, HttpClient);

            var dto = new CurrencyExchangeRateDto
            {
                FromCurrencyCode = "USD",
                ToCurrencyCode = "EUR",
                ExchangeRate = 12.3m,
                LastRefreshed = DateTime.UtcNow,
                AskPrice = 1.1m,
                BidPrice = 1.2m
            };

            // Act
            var result = await service.CreateCurrencyExchangeRateAsync(dto, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(dto);
        }

        [Fact]
        public async Task UpdateCurrencyExchangeRateAsync_Success()
        {
            // Arrange
            var entity = new CurrencyExchangeRate
            {
                FromCurrencyCode = "USD",
                ToCurrencyCode = "EUR",
                ExchangeRate = 10.3m,
                LastRefreshed = DateTime.UtcNow,
                AskPrice = 3.3m,
                BidPrice = 3.4m,
                Id = 1
            };

            MockedCurrencyExchangeRateRepository
                .Setup(w => w.GetCurrencyExchangeRateAsync(It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(entity);

            var updatedEntity = new CurrencyExchangeRate
            {
                FromCurrencyCode = "USD",
                ToCurrencyCode = "EUR",
                ExchangeRate = 11.3m,
                LastRefreshed = DateTime.UtcNow,
                AskPrice = 1.3m,
                BidPrice = 1.4m,
                Id = 1
            };

            MockedCurrencyExchangeRateRepository
                .Setup(w => w.UpdateCurrencyExchangeRateAsync(It.IsAny<CurrencyExchangeRate>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(updatedEntity);

            var service = new CurrencyExchangeRateService(Mock.Of<ILogger<CurrencyExchangeRateService>>(), MockedCurrencyExchangeRateRepository.Object, Mapper, HttpClient);

            var dto = new CurrencyExchangeRateDto
            {
                FromCurrencyCode = "USD",
                ToCurrencyCode = "EUR",
                ExchangeRate = 11.3m,
                LastRefreshed = updatedEntity.LastRefreshed,
                AskPrice = 1.3m,
                BidPrice = 1.4m
            };

            // Act
            var result = await service.UpdateCurrencyExchangeRateAsync(dto, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(dto);
        }

        [Fact]
        public async Task UpdateCurrencyExchangeRateAsync_Failure_WhenEntityDoesntExist()
        {
            // Arrange
            MockedCurrencyExchangeRateRepository
                .Setup(w => w.UpdateCurrencyExchangeRateAsync(It.IsAny<CurrencyExchangeRate>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(null as CurrencyExchangeRate);

            var service = new CurrencyExchangeRateService(Mock.Of<ILogger<CurrencyExchangeRateService>>(), MockedCurrencyExchangeRateRepository.Object, Mapper, HttpClient);

            var dto = new CurrencyExchangeRateDto
            {
                FromCurrencyCode = "USD",
                ToCurrencyCode = "EUR",
                ExchangeRate = 11.3m,
                LastRefreshed = DateTime.UtcNow,
                AskPrice = 1.3m,
                BidPrice = 1.4m
            };

            // Act
            var result = await service.UpdateCurrencyExchangeRateAsync(dto, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task DeleteCurrencyExchangeRateAsync_Success()
        {
            MockedCurrencyExchangeRateRepository
                .Setup(w => w.DeleteCurrencyExchangeRateAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var service = new CurrencyExchangeRateService(Mock.Of<ILogger<CurrencyExchangeRateService>>(), MockedCurrencyExchangeRateRepository.Object, Mapper, HttpClient);

            // Act
            var result = await service.DeleteCurrencyExchangeRateAsync("USD", "EUR", CancellationToken.None);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteCurrencyExchangeRateAsync_Failure_WhenEntityDoesntExist()
        {
            MockedCurrencyExchangeRateRepository
                .Setup(w => w.DeleteCurrencyExchangeRateAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var service = new CurrencyExchangeRateService(Mock.Of<ILogger<CurrencyExchangeRateService>>(), MockedCurrencyExchangeRateRepository.Object, Mapper, HttpClient);

            // Act
            var result = await service.DeleteCurrencyExchangeRateAsync("USD", "EUR", CancellationToken.None);

            // Assert
            result.Should().BeFalse();
        }

        private static IMapper GetMapper()
        {
            var profileTypes = typeof(CurrencyExchangeRateProfile).Assembly
                .GetTypes()
                .Where(t => typeof(Profile).IsAssignableFrom(t));

            var config = new MapperConfiguration(cfg =>
            {
                foreach (var profileType in profileTypes)
                {
                    var profile = Activator.CreateInstance(profileType) as Profile;
                    cfg.AddProfile(profile);
                }
            });

            return config.CreateMapper();
        }

        private static HttpClient GetHttpClient() 
        {
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(AlphaVantageResponse)
                });

            return new HttpClient(mockMessageHandler.Object)
            {
                BaseAddress = new Uri("http://localhost")
            };
        }
    }
}
