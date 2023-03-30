namespace TennisBookings.Areas.Admin.Models;

public class CourtMaintenanceViewModel
{
	public string? CourtName { get; set; }

	public string? Title { get; set; }

	public bool CourtIsClosed { get; set; }

	public DateTime StartDateTime { get; set; }

	public DateTime EndDateTime { get; set; }
}
