
string data = "Can we find this character: a\u030a";

int index = data.IndexOf("\u00e5", StringComparison.Ordinal); // å

Console.WriteLine(index);













Console.ReadLine();