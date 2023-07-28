
using System.Globalization;
using System.Text;

Console.OutputEncoding = Encoding.Unicode;

var culture = CultureInfo.CreateSpecificCulture("en-US");
culture.NumberFormat.NumberGroupSeparator = " ";

decimal number = 1_000.50m;

string numberAsString =
    number.ToString("#,#.#", culture);

string numberAsStringSwedish =
    number.ToString("#,#.#", CultureInfo.CreateSpecificCulture("sv-SE"));

Console.WriteLine(numberAsString);
Console.WriteLine(numberAsStringSwedish);
Console.ReadLine();