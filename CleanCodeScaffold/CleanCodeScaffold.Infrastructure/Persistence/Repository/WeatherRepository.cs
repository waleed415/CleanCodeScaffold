using CleanCodeScaffold.Domain.Entities;
using CleanCodeScaffold.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanCodeScaffold.Infrastructure.Persistence.Repository
{
    public class WeatherRepository : BaseRepository<Weather>, IWeatherRepository
    {
        public WeatherRepository(AppDBContext context):base(context)
        {
            
        }
    }
}
