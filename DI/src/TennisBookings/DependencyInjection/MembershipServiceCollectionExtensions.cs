using TennisBookings.Services.Membership;

namespace TennisBookings.DependencyInjection
{
	public static class MembershipServiceCollectionExtensions
	{
		public static IServiceCollection AddMembershipServices(this IServiceCollection services)
		{
			services.AddTransient<IMembershipAdvertBuilder, MembershipAdvertBuilder>();
			services.AddSingleton<IMembershipAdvert>(sp =>
			{
				var builder = sp.GetRequiredService<IMembershipAdvertBuilder>();
				builder.WithDiscount(10m);
				var advert = builder.Build();
				return advert;
			});

			return services;
		}
	}
}
