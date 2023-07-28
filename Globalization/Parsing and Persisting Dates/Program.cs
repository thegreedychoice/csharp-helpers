

using System.Globalization;

DateTimeOffset date = new DateTimeOffset(
    2021, 05, 10,
    18, 30, 59,
    TimeSpan.FromHours(8));

string dateAsString = date.ToString("O",
    new CultureInfo("sv-SE"));

Console.WriteLine(dateAsString);

DateTime parsedDate = DateTime.Parse(dateAsString,
    CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);

DateTimeOffset parsedDateAsDateTimeOffset =
    DateTimeOffset.Parse(dateAsString);

Console.WriteLine(parsedDate);
Console.WriteLine(parsedDateAsDateTimeOffset);

Console.WriteLine(parsedDateAsDateTimeOffset.ToLocalTime());


Console.ReadLine();