using FluentAssertions;
using Jaunts.Core.Api.Tests.Acceptance.Brokers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jaunts.Core.Api.Tests.Acceptance.Api.WeatherForecastTest
{
    [Collection(nameof(ApiTestCollection))]
    public class WeatherForecastApiTest
    {
        private readonly JauntsApiBroker jauntsApiBroker;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

      
    public WeatherForecastApiTest(JauntsApiBroker jauntsApiBroker) =>
            this.jauntsApiBroker = jauntsApiBroker;

        [Fact]
        public async Task GetWeatherForecast()
        {
            //Given

            var expectedWeatherForecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now),
                TemperatureC = 1,
                Summary = "test"
            })
            .ToArray();

            //When
            List<WeatherForecast> weatherForecasts = await this.jauntsApiBroker.GetWeatherForecast();

            //Then
            expectedWeatherForecasts.Should().BeEquivalentTo(weatherForecasts);
        }
    }
}
