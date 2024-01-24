using CleanCodeScaffold.Application.Dtos;
using CleanCodeScaffold.Application.Handlers.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanCodeScaffold.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : BaseController<WeatherVM>
    {
        public WeatherController(IWeatherHandler weatherHandler):base(weatherHandler) 
        {
        }
    }
}
