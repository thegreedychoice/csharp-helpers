
using System.Globalization;

string first    = "\u00e5";         // å
string second   = "\u0061\u030a";   // a ̊

bool result = string.Equals(first, second);

int sortOrderOrdinal = string.Compare(first, second, StringComparison.Ordinal);

int sortOrderInvariant
    = string.Compare(first, second, StringComparison.InvariantCulture);

Console.WriteLine(sortOrderOrdinal);
Console.WriteLine(sortOrderInvariant);

int sortOrderCurrentCulture = CultureInfo.CurrentCulture.CompareInfo
    .Compare(first, second);

Console.WriteLine(sortOrderCurrentCulture);

Console.ReadLine();