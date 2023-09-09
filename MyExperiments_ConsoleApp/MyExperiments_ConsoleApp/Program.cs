using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyExperiments_ConsoleApp.ConcurrencyWithStaticDictionary;

Console.WriteLine("Program:Main - Start");

// create hosting object and DI layer
using var host = CreateHostBuilder().Build();

Console.WriteLine("Program:Main - Starting Application");
Console.WriteLine();

await ConcurrencyWithStaticSimulation.RunScopedSimulationStaticConcurrency(host);

Console.WriteLine("\n------------------------------------------\n");

await ConcurrencyWithStaticSimulation.RunSingletonSimulationStaticConcurrency(host);

Console.WriteLine();
Console.WriteLine("Program:Main - End");



// implementation of 'CreateHostBuilder' static method and create host object
IHostBuilder CreateHostBuilder()
{
    return Host.CreateDefaultBuilder()
        .ConfigureServices((_, services) =>
        {
            services.AddScoped<IMyScopedServiceA, MyScopedServiceA>();
            services.AddSingleton<IMySingletonService, MySingletonService>();
            services.AddScoped<IMyScopedServiceB, MyScopedServiceB>();
            services.AddScoped<IMyScopedServiceC, MyScopedServiceC>();
        });
}

