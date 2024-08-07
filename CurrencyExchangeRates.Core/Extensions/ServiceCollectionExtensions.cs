using CurrencyExchangeRates.Core.Mapping;
using CurrencyExchangeRates.Core.Services;
using CurrencyExchangeRates.Core.Validators;
using CurrencyExchangeRates.Database;
using CurrencyExchangeRates.Database.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace CurrencyExchangeRates.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCurrencyExchangeRateDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("CurrencyExchangeRate") ?? throw new NullReferenceException("Missing connection string for CurrencyExchangeRate");

            services.AddDbContext<CurrencyExchangeRateDbContext>(opts =>
            {
                var migrationsAssembly = typeof(CurrencyExchangeRateDbContext).Assembly.GetName().Name;

                opts.UseSqlServer(connectionString, sql =>
                {
                    sql.MigrationsAssembly(migrationsAssembly);
                    sql.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                });
            });
        }

        public static void AddServiceDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            var alphaVantageUrl = configuration["AlphaVantage:Url"] ?? throw new NullReferenceException("Missing configuration parameter for AlphaVantage:Url");
            var alphaVantageApiKey = configuration["AlphaVantage:ApiKey"] ?? throw new NullReferenceException("Missing configuration parameter for AlphaVantage:ApiKey");

            alphaVantageUrl = QueryHelpers.AddQueryString(alphaVantageUrl, "apikey", alphaVantageApiKey);

            services.AddTransient<ICurrencyExchangeRateRepository, CurrencyExchangeRateRepository>();
            services.AddTransient<ICurrencyExchangeRateService, CurrencyExchangeRateService>();
            services.AddValidatorsFromAssemblyContaining<CurrencyExchangeRateDtoValidator>();
            services.AddAutoMapper(typeof(CurrencyExchangeRateProfile));
            services.AddHttpClient<ICurrencyExchangeRateService, CurrencyExchangeRateService>(client =>
            {
                client.BaseAddress = new Uri(alphaVantageUrl);
            })
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPolicy());
        }

        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
        static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(3, TimeSpan.FromSeconds(30));
        }
    }
}
