using CleanCodeScaffold.Application.Dtos;
using CleanCodeScaffold.Application.Handlers.Interface;
using CleanCodeScaffold.Application.Mappers;
using CleanCodeScaffold.Application.Validators;
using CleanCodeScaffold.Domain.Entities;
using CleanCodeScaffold.Domain.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanCodeScaffold.Application.Handlers.Implimentation
{
    public class WeatherHandler : BaseHandler<WeatherVM, Weather>, IWeatherHandler
    {
        public WeatherHandler(IWeatherRepository repo, IValidator<WeatherVM> validator, FilterValidator<WeatherVM> filterValidator) : base(repo, WeatherMapper.Mapper, validator, filterValidator)
        {
            
        }
    }
}
