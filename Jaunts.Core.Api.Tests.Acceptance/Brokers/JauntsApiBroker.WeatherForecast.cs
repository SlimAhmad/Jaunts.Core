using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jaunts.Core.Api.Tests.Acceptance.Brokers
{
    public partial class JauntsApiBroker
    {
        private const string Url  = "api/weatherforecast";

        public async ValueTask<List<WeatherForecast>> GetWeatherForecast() =>
            await apiFactoryClient.GetContentAsync<List<WeatherForecast>>(Url);
        
    }
}
