
using System.Globalization;

string[] names = {  "Örjan", "Zoe", "filip",
                    "Olof", "chris", "Chloë", "Mila", "Elise",
                     "John", "Åsa", "Anna", "Sofie", "Fèlip" };


CultureInfo.CurrentCulture = new CultureInfo("en-US");

Array.Sort(names, StringComparer.OrdinalIgnoreCase);

foreach(var name in names)
{
    Console.WriteLine(name);
}

Console.ReadLine();