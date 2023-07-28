
using System.Globalization;
using System.Text;

Console.OutputEncoding = Encoding.Unicode;

var culture = CultureInfo.CreateSpecificCulture("en-US");

decimal number = 1_000_000.50m;

string numberAsString = number.ToString("C0", culture);

string numberAsSwedishKronors =
    (number * 11)
    .ToString("C", CultureInfo.CreateSpecificCulture("sv-SE"));

Console.WriteLine(numberAsString);
Console.WriteLine(numberAsSwedishKronors);

Console.ReadLine();