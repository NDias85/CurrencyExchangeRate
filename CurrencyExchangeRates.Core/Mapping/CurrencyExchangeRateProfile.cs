using AutoMapper;
using CurrencyExchangeRates.Models.DTO;
using CurrencyExchangeRates.Models.Entities;
using CurrencyExchangeRates.Models.External;

namespace CurrencyExchangeRates.Core.Mapping
{
    public class CurrencyExchangeRateProfile : Profile
    {
        public CurrencyExchangeRateProfile()
        {
            CreateMap<CurrencyExchangeRateDto, CurrencyExchangeRate>()
                .ForMember(dest => dest.Id, options => options.Ignore())
                .ReverseMap();

            CreateMap<RealTimeCurrencyExchangeRate, CurrencyExchangeRate>()
                .ForMember(dest => dest.Id, options => options.Ignore())
                .ForMember(dest => dest.LastRefreshed, options => options.MapFrom(m => DateTime.Parse(m.LastRefreshed)));
        }
    }
}
