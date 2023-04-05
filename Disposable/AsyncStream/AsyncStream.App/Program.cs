using System;
using System.Threading.Tasks;

namespace AsyncStream.App
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("** Starting **");

            await using (var rand = new RandomStringGenerator())
            {
                await foreach (var s in rand.Get(50))
                {
                    Console.WriteLine(s);
                }
            }

            Console.WriteLine("** Done **");
        }
    }
}
