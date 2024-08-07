using CurrencyExchangeRates.Controllers;
using CurrencyExchangeRates.Core.Services;
using CurrencyExchangeRates.Models.DTO;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CurrencyExchangeRates.UnitTests
{
    public class CurrencyExchangeRateControllerTests
    {
        private readonly Mock<ICurrencyExchangeRateService> MockedCurrencyExchangeRateService = new();

        [Fact]
        public async Task CurrencyExchangeRateGetAsync_Success()
        {
            // Arrange
            var expectedResult = new CurrencyExchangeRateDto()
            {
                ExchangeRate = 0.96m
            };

            MockedCurrencyExchangeRateService
                .Setup(w => w.GetCurrencyExchangeRateAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            var controller = new CurrencyExchangeRateController(MockedCurrencyExchangeRateService.Object);

            // Act
            var response = await controller.GetAsync("USD", "GBP", CancellationToken.None);

            // Assert
            response.Value!.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task CurrencyExchangeRateGetAsync_NotFound_WhenEntityDoesntExist()
        {
            // Arrange
            var controller = new CurrencyExchangeRateController(MockedCurrencyExchangeRateService.Object);

            // Act
            var response = await controller.GetAsync("USD", "GBP", CancellationToken.None);

            // Assert
            response.Value!.Should().BeNull();
        }

        [Fact]
        public async Task CurrencyExchangeRatePostAsync_Success()
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

            var controller = new CurrencyExchangeRateController(MockedCurrencyExchangeRateService.Object);

            // Act
            var response = await controller.PostAsync(expectedResult, CancellationToken.None);

            // Assert
            response.Value!.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task CurrencyExchangeRatePostAsync_BadRequest_WhenDataIsInvalid()
        {
            // Arrange
            var requestData = new CurrencyExchangeRateDto()
            {
                AskPrice = 1.23m,
                BidPrice = 1.44m,
                LastRefreshed = DateTime.UtcNow,
                ExchangeRate = 0.96m
            };

            var controller = new CurrencyExchangeRateController(MockedCurrencyExchangeRateService.Object);

            // Act
            var response = await controller.PostAsync(requestData, CancellationToken.None);

            // Assert
            var result = response.Result as BadRequestObjectResult;
            result!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result!.Value.Should().NotBeNull();
        }

        [Fact]
        public async Task CurrencyExchangeRatePutAsync_Success()
        {
            // Arrange
            var expectedResult = new CurrencyExchangeRateDto()
            {
                AskPrice = 1.23m,
                BidPrice = 1.44m,
                LastRefreshed = DateTime.UtcNow,
                ExchangeRate = 0.96m
            };

            MockedCurrencyExchangeRateService
                .Setup(w => w.UpdateCurrencyExchangeRateAsync(It.IsAny<CurrencyExchangeRateDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            var controller = new CurrencyExchangeRateController(MockedCurrencyExchangeRateService.Object);

            // Act
            var response = await controller.PutAsync("USD", "GBP", expectedResult, CancellationToken.None);

            // Assert
            response.Value!.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task CurrencyExchangeRatePutAsync_BadRequest_WhenDataIsInvalid()
        {
            // Arrange
            var requestData = new CurrencyExchangeRateDto()
            {
                AskPrice = 1.23m,
                BidPrice = 1.44m,
                LastRefreshed = DateTime.UtcNow,
                ExchangeRate = 0.96m
            };

            var controller = new CurrencyExchangeRateController(MockedCurrencyExchangeRateService.Object);

            // Act
            var response = await controller.PutAsync("", "", requestData, CancellationToken.None);

            // Assert
            var result = response.Result as BadRequestObjectResult;
            result!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result!.Value.Should().NotBeNull();
        }

        [Fact]
        public async Task CurrencyExchangeRateDeleteAsync_Success()
        {
            // Arrange
            MockedCurrencyExchangeRateService
                .Setup(w => w.DeleteCurrencyExchangeRateAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var controller = new CurrencyExchangeRateController(MockedCurrencyExchangeRateService.Object);

            // Act
            var response = await controller.DeleteAsync("USD", "GBP", CancellationToken.None);

            // Assert
            response.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

        [Fact]
        public async Task CurrencyExchangeRateDeleteAsync_NotFound_WhenEntityDoesntExist()
        {
            // Arrange
            var expectedResult = new CurrencyExchangeRateDto()
            {
                ExchangeRate = 0.96m
            };

            var controller = new CurrencyExchangeRateController(MockedCurrencyExchangeRateService.Object);

            // Act
            var response = await controller.DeleteAsync("USD", "GBP", CancellationToken.None);

            // Assert
            response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }
    }
}