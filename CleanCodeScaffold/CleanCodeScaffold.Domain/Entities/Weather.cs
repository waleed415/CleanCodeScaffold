using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanCodeScaffold.Domain.Entities
{
    public class Weather : BaseEntity
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }
        public string? Summary { get; set; }
    }
}
