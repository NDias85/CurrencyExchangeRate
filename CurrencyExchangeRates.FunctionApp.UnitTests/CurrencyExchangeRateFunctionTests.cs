using CurrencyExchangeRates.Core.Services;
using CurrencyExchangeRates.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using FluentAssertions;
using Moq;

namespace CurrencyExchangeRates.FunctionApp.UnitTests
{
    public class CurrencyExchangeRateFunctionTests
    {
        private readonly Mock<ICurrencyExchangeRateService> MockedCurrencyExchangeRateService = new();

        [Fact]
        public async Task CurrencyExchangeRateGet_Success()
        {
            // Arrange
            var expectedResult = new CurrencyExchangeRateDto()
            {
                ExchangeRate = 0.96m
            };

            MockedCurrencyExchangeRateService
                .Setup(w => w.GetCurrencyExchangeRateAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            var httpRequestData = MockHttpRequestData.Create();

            var function = new CurrencyExchangeRateFunction(MockedCurrencyExchangeRateService.Object);

            // Act
            var response = await function.RunGet(httpRequestData, "USD", "GBP", CancellationToken.None);

            // Assert
            var result = response as OkObjectResult;
            result!.StatusCode.Should().Be(StatusCodes.Status200OK);
            result!.Value.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task CurrencyExchangeRateGet_NotFound_WhenEntityDoesntExist()
        {
            // Arrange
            var httpRequestData = MockHttpRequestData.Create();

            var function = new CurrencyExchangeRateFunction(MockedCurrencyExchangeRateService.Object);

            // Act
            var response = await function.RunGet(httpRequestData, "USD", "GBP", CancellationToken.None);

            // Assert
            var result = response as NotFoundResult;
            result!.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task CurrencyExchangeRatePost_Success()
        {
            // Arrange
            var expectedResult = new CurrencyExchangeRateDto()
            {
                FromCurrencyCode = "EUR",
                ToCurrencyCode = "USD",
                AskPrice = 1.23m,
                BidPrice = 1.44m,
                LastRefreshed = DateTime.UtcNow,
                ExchangeRate = 0.96m
            };

            MockedCurrencyExchangeRateService
                .Setup(w => w.CreateCurrencyExchangeRateAsync(It.IsAny<CurrencyExchangeRateDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            var httpRequestData = MockHttpRequestData.Create(expectedResult);

            var function = new CurrencyExchangeRateFunction(MockedCurrencyExchangeRateService.Object);

            // Act
            var response = await function.RunPost(httpRequestData, Mock.Of<Microsoft.Extensions.Logging.ILogger>(), CancellationToken.None);

            // Assert
            var result = response as OkObjectResult;
            result!.StatusCode.Should().Be(StatusCodes.Status200OK);
            result!.Value.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task CurrencyExchangeRatePost_BadRequest_WhenDataIsInvalid()
        {
            // Arrange
            var requestData = new CurrencyExchangeRateDto()
            {
                AskPrice = 1.23m,
                BidPrice = 1.44m,
                LastRefreshed = DateTime.UtcNow,
                ExchangeRate = 0.96m
            };

            var httpRequestData = MockHttpRequestData.Create(requestData);

            var function = new CurrencyExchangeRateFunction(MockedCurrencyExchangeRateService.Object);

            // Act
            var response = await function.RunPost(httpRequestData, Mock.Of<Microsoft.Extensions.Logging.ILogger>(), CancellationToken.None);

            // Assert
            var result = response as BadRequestObjectResult;
            result!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result!.Value.Should().NotBeNull();
        }

        [Fact]
        public async Task CurrencyExchangeRatePut_Success()
        {
            // Arrange
            var expectedResult = new CurrencyExchangeRateDto()
            {
                FromCurrencyCode = "EUR",
                ToCurrencyCode = "USD",
                AskPrice = 1.23m,
                BidPrice = 1.44m,
                LastRefreshed = DateTime.UtcNow,
                ExchangeRate = 0.96m
            };

            MockedCurrencyExchangeRateService
                .Setup(w => w.UpdateCurrencyExchangeRateAsync(It.IsAny<CurrencyExchangeRateDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            var httpRequestData = MockHttpRequestData.Create(expectedResult);

            var function = new CurrencyExchangeRateFunction(MockedCurrencyExchangeRateService.Object);

            // Act
            var response = await function.RunPut(httpRequestData, "EUR", "USD", Mock.Of<Microsoft.Extensions.Logging.ILogger>(), CancellationToken.None);

            // Assert
            var result = response as OkObjectResult;
            result!.StatusCode.Should().Be(StatusCodes.Status200OK);
            result!.Value.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task CurrencyExchangeRatePut_BadRequest_WhenDataIsInvalid()
        {
            // Arrange
            var requestData = new CurrencyExchangeRateDto()
            {
                AskPrice = 1.23m,
                BidPrice = 1.44m,
                LastRefreshed = DateTime.UtcNow,
                ExchangeRate = 0.96m
            };

            var httpRequestData = MockHttpRequestData.Create(requestData);

            var function = new CurrencyExchangeRateFunction(MockedCurrencyExchangeRateService.Object);

            // Act
            var response = await function.RunPut(httpRequestData, "", "", Mock.Of<Microsoft.Extensions.Logging.ILogger>(), CancellationToken.None);

            // Assert
            var result = response as BadRequestObjectResult;
            result!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result!.Value.Should().NotBeNull();
        }

        [Fact]
        public async Task CurrencyExchangeRateDelete_Success()
        {
            // Arrange
            MockedCurrencyExchangeRateService
                .Setup(w => w.DeleteCurrencyExchangeRateAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var httpRequestData = MockHttpRequestData.Create();

            var function = new CurrencyExchangeRateFunction(MockedCurrencyExchangeRateService.Object);

            // Act
            var response = await function.RunDelete(httpRequestData, "USD", "GBP", CancellationToken.None);

            // Assert
            response.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

        [Fact]
        public async Task CurrencyExchangeRateDelete_NotFound_WhenEntityDoesntExist()
        {
            // Arrange
            var expectedResult = new CurrencyExchangeRateDto()
            {
                ExchangeRate = 0.96m
            };

            var httpRequestData = MockHttpRequestData.Create();

            var function = new CurrencyExchangeRateFunction(MockedCurrencyExchangeRateService.Object);

            // Act
            var response = await function.RunDelete(httpRequestData, "USD", "GBP", CancellationToken.None);

            // Assert
            response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }
    }
}