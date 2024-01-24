using CleanCodeScaffold.Application.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanCodeScaffold.Application.Validators
{
    public class WeatherValidator : AbstractValidator<WeatherVM>
    {
        public WeatherValidator() {
            RuleFor(x => x.TemperatureC).GreaterThanOrEqualTo(-25).WithMessage("Temprature must be greate then or equal to -25");
        }
    }
}
