using Microsoft.Extensions.Options;

namespace TennisBookings.Services.Bookings.Rules
{
	public class MaxPeakTimeBookingLengthRule : ICourtBookingRule
	{
		private readonly ClubConfiguration _clubConfiguration;
		private readonly BookingConfiguration _bookingConfiguration;

		public MaxPeakTimeBookingLengthRule(IOptions<ClubConfiguration> clubOptions,
			IOptions<BookingConfiguration> options)
		{
			_clubConfiguration = clubOptions.Value;
			_bookingConfiguration = options.Value;
		}

		public Task<bool> CompliesWithRuleAsync(CourtBooking booking)
		{
			if (booking.EndDateTime.Hour < _clubConfiguration.PeakStartHour)
				return Task.FromResult(true);

			var peakHours = 0;
			for (var hour = booking.StartDateTime.Hour; hour < booking.EndDateTime.Hour; hour++)
			{
				if (hour >= _clubConfiguration.PeakStartHour && hour <= _clubConfiguration.PeakEndHour)
				{
					peakHours++;
				}
			}

			return Task.FromResult(peakHours <= _bookingConfiguration.MaxPeakBookingLengthInHours);
		}

		public string ErrorMessage => "The court booking is too long";
	}
}
