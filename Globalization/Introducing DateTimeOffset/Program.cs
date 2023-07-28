
#region DateTimeOffset
DateTimeOffset now = DateTimeOffset.Now;

DateTimeOffset otherDate = now.ToOffset(TimeSpan.FromHours(1));

DateTimeOffset utcDate = otherDate.ToUniversalTime();
#endregion

#region Convert to local time
// Convert this to the CORRECT local time!

DateTime date = new DateTime(2020, 01, 01,
                             13, 30, 0,
                             DateTimeKind.Utc);

var localDate = TimeZoneInfo.ConvertTimeFromUtc(date,
    TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"));

#endregion

Console.ReadLine();