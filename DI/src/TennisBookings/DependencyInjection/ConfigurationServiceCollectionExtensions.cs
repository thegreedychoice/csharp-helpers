using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace TennisBookings.DependencyInjection
{
	public static class ConfigurationServiceCollectionExtensions
	{
		public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration config)
		{
			services.Configure<ClubConfiguration>(config.GetSection("ClubSettings"));
			services.Configure<BookingConfiguration>(config.GetSection("CourtBookings"));
			services.Configure<FeaturesConfiguration>(config.GetSection("Features"));
			services.Configure<MembershipConfiguration>(config.GetSection("Membership"));

			services.TryAddSingleton<IBookingConfiguration>(sp =>
				sp.GetRequiredService<IOptions<BookingConfiguration>>().Value); // forwarding via implementation factory

			return services;
		}
	}
}
