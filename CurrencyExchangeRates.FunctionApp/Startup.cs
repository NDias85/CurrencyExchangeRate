using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CurrencyExchangeRates.Core.Extensions;

[assembly: FunctionsStartup(typeof(CurrencyExchangeRates.FunctionApp.Startup))]

namespace CurrencyExchangeRates.FunctionApp;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        var configuration = builder.Services.BuildServiceProvider().GetService<IConfiguration>();

        builder.Services.AddCurrencyExchangeRateDbContext(configuration!);
        builder.Services.AddServiceDependencies(configuration!);
    }
}