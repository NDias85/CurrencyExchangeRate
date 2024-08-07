using CurrencyExchangeRates.Database.Repositories;
using CurrencyExchangeRates.Models.Entities;
using FluentAssertions;

namespace CurrencyExchangeRates.Database.UnitTests
{
    public class CurrencyExchangeRateRepositoryTests : BaseRepositoryTests
    {
        [Fact]
        public async Task GetCurrencyExchangeRateAsync_Success()
        {
            // Arrange
            var expectedResult = new CurrencyExchangeRate 
            {
                FromCurrencyCode = "USD",
                ToCurrencyCode = "EUR",
                ExchangeRate = 12.3m,
                LastRefreshed = DateTime.UtcNow,
                AskPrice = 1.1m,
                BidPrice = 1.2m
            };
            await _context.CurrencyExchangeRates.AddAsync(expectedResult);
            await _context.SaveChangesAsync();

            var repository = new CurrencyExchangeRateRepository(_context);

            // Act
            var result = await repository.GetCurrencyExchangeRateAsync(expectedResult.FromCurrencyCode, expectedResult.ToCurrencyCode, CancellationToken.None);
            
            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResult, options => options.Excluding(w => w.Id));
        }

        [Fact]
        public async Task CreateCurrencyExchangeRateAsync_Success()
        {
            // Arrange
            var repository = new CurrencyExchangeRateRepository(_context);

            var entity = new CurrencyExchangeRate
            {
                FromCurrencyCode = "USD",
                ToCurrencyCode = "EUR",
                ExchangeRate = 12.3m,
                LastRefreshed = DateTime.UtcNow,
                AskPrice = 1.1m,
                BidPrice = 1.2m
            };

            // Act
            var result = await repository.CreateCurrencyExchangeRateAsync(entity, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(entity, options => options.Excluding(w => w.Id));
            result.Should().BeEquivalentTo(await _context.CurrencyExchangeRates.FindAsync(result.Id));
        }

        [Fact]
        public async Task UpdateCurrencyExchangeRateAsync_Success()
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
            await _context.CurrencyExchangeRates.AddAsync(entity);
            await _context.SaveChangesAsync();

            var repository = new CurrencyExchangeRateRepository(_context);

            entity.ExchangeRate = 22.67m;
            entity.AskPrice = 2.1m;
            entity.BidPrice = 2.2m;
            entity.LastRefreshed = DateTime.UtcNow;

            // Act
            var result = await repository.UpdateCurrencyExchangeRateAsync(entity, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(entity);
            result.Should().BeEquivalentTo(await _context.CurrencyExchangeRates.FindAsync(entity.Id));
        }

        [Fact]
        public async Task UpdateCurrencyExchangeRateAsync_FailureOnNonExistingEntity()
        {
            // Arrange
            var repository = new CurrencyExchangeRateRepository(_context);

            var entity = new CurrencyExchangeRate
            {
                FromCurrencyCode = "USD",
                ToCurrencyCode = "EUR",
                ExchangeRate = 12.3m,
                LastRefreshed = DateTime.UtcNow,
                AskPrice = 1.1m,
                BidPrice = 1.2m
            };

            // Act
            var result = await repository.UpdateCurrencyExchangeRateAsync(entity, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task DeleteCurrencyExchangeRateAsync_Success()
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
            await _context.CurrencyExchangeRates.AddAsync(entity);
            await _context.SaveChangesAsync();

            var repository = new CurrencyExchangeRateRepository(_context);

            // Act
            var result = await repository.DeleteCurrencyExchangeRateAsync(entity.FromCurrencyCode, entity.ToCurrencyCode, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteCurrencyExchangeRateAsync_FailureOnNonExistingEntity()
        {
            // Arrange
            var repository = new CurrencyExchangeRateRepository(_context);

            var entity = new CurrencyExchangeRate
            {
                FromCurrencyCode = "USD",
                ToCurrencyCode = "EUR",
                ExchangeRate = 12.3m,
                LastRefreshed = DateTime.UtcNow,
                AskPrice = 1.1m,
                BidPrice = 1.2m
            };

            // Act
            var result = await repository.DeleteCurrencyExchangeRateAsync(entity.FromCurrencyCode, entity.ToCurrencyCode, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
        }
    }
}
