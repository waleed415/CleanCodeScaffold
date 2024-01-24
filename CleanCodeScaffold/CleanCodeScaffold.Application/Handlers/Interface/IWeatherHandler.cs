using CleanCodeScaffold.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanCodeScaffold.Application.Handlers.Interface
{
    public interface IWeatherHandler : IBaseHandler<WeatherVM>
    {
    }
}
