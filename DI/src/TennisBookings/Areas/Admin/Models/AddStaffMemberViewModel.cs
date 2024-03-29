using System.ComponentModel;

namespace TennisBookings.Areas.Admin.Models;

public class AddStaffMemberViewModel
{
	[DisplayName("First name")]
	public string? FirstName { get; set; }

	[DisplayName("Last name")]
	public string? LastName { get; set; }

	public string? Role { get; set; }
}
