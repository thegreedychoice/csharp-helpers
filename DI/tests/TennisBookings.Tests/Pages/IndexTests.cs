using Microsoft.Extensions.Options;
using TennisBookings.Configuration;

namespace TennisBookings.Tests.Pages;

public class IndexTests
{
	[Fact]
	public async Task ReturnsExpectedViewModel_WhenWeatherIsSun()
	{
		var sut = new IndexModel(
			new SunForecaster(),
			NullLogger<IndexModel>.Instance,
			new EnabledConfig());

		await sut.OnGet();

		Assert.Contains("It's sunny right now.", sut.WeatherDescription);
	}

	[Fact]
	public async Task ReturnsExpectedViewModel_WhenWeatherIsRain()
	{
		var sut = new IndexModel(
			new RainForecaster(),
			NullLogger<IndexModel>.Instance,
			new EnabledConfig());

		await sut.OnGet();

		Assert.Contains("We're sorry but it's raining here.", sut.WeatherDescription);
	}

	private class EnabledConfig : IOptionsSnapshot<FeaturesConfiguration>
	{
		public FeaturesConfiguration Value => new FeaturesConfiguration
		{
			EnableWeatherForecast = true
		};

		public FeaturesConfiguration Get(string name)
		{
			throw new NotImplementedException();
		}
	}

	private class SunForecaster : IWeatherForecaster
	{
		public Task<WeatherResult> GetCurrentWeatherAsync(string city)
		{
			return Task.FromResult(new WeatherResult
			{
				City = city,
				Weather = new WeatherCondition { Summary = "Sun" }
			});
		}
	}

	private class RainForecaster : IWeatherForecaster
	{
		public Task<WeatherResult> GetCurrentWeatherAsync(string city)
		{
			return Task.FromResult(new WeatherResult
			{
				City = city,
				Weather = new WeatherCondition { Summary = "Rain" }
			});
		}
	}
}
