using AutoMapper;
using CleanCodeScaffold.Application.Dtos;
using CleanCodeScaffold.Domain.Entities;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanCodeScaffold.Application.Mappers
{
    internal class WeatherMapper 
    {
        private static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() => {
            var config = new MapperConfiguration(cfg => {
                cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
                cfg.AddProfile<WeatherProfile>();
            });
            var mapper = config.CreateMapper();
            return mapper;
        });

        public static IMapper Mapper => Lazy.Value;
    }

    internal class WeatherProfile : Profile
    {
        public WeatherProfile()
        {
            CreateMap<Weather, WeatherVM>().ReverseMap();
            CreateMap<WeatherVM, Weather>().ReverseMap();
        }
    }
}
