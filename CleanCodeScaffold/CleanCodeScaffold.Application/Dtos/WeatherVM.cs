using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanCodeScaffold.Application.Dtos
{
    public class WeatherVM
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }
        public string? Summary { get; set; }
    }
}
