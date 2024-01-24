using CleanCodeScaffold.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanCodeScaffold.Domain.Interfaces
{
    public interface IWeatherRepository : IBaseRepository<Weather>
    {
    }
}
