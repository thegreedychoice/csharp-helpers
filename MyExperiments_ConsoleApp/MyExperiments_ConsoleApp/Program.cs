using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyExperiments_ConsoleApp.ConcurrencyWithStaticDictionary;

Console.WriteLine("Program:Main - Start");

// create hosting object and DI layer
using var host = CreateHostBuilder().Build();

Console.WriteLine("Program:Main - Starting Application");
Console.WriteLine();

await RunScopedSimulationStaticConcurrency(host);

Console.WriteLine("\n------------------------------------------\n");

await RunSingletonSimulationStaticConcurrency(host);

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

async Task RunScopedSimulationStaticConcurrency(IHost hostInner)
{
    var scopedTasks = new List<Task>();

    const int scopedServiceATasksCount = 3;
    const int scopedServiceBTasksCount = 2;
    const int scopedServiceCTasksCount = 1;

    var transactionIds = new List<string>();
    for (var counter = 0; counter < scopedServiceATasksCount; counter++)
    {
        var x = Guid.NewGuid().ToString();
        transactionIds.Add(x);
    }
    
    for (var i = 1; i <= scopedServiceATasksCount; i++)
    {
        // capture value of i locally
        var currentInstanceNo = i;
        
        // create a service scope
        var scope = hostInner.Services.CreateScope();
        var serviceProvider = scope.ServiceProvider;

        // do scoped work 
        scopedTasks.Add(Task.Run(async () =>
        {
            // resolve the scoped service A through DI
            var scopedServiceA = serviceProvider.GetRequiredService<IMyScopedServiceA>();
            await scopedServiceA.AddToCache(transactionIds[currentInstanceNo-1], currentInstanceNo, currentInstanceNo);
        }));
    }
    
    for (var i = 1; i <= scopedServiceBTasksCount; i++)
    {
        // capture value of i locally
        var currentInstanceNo = i;
        
        
        // create a service scope
        var scope = hostInner.Services.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        
        scopedTasks.Add(Task.Run(async () =>
        {
            // resolve the scoped service B through DI
            var scopedServiceB = serviceProvider.GetRequiredService<IMyScopedServiceB>();
            await scopedServiceB.AddToCache(transactionIds[currentInstanceNo-1], currentInstanceNo, currentInstanceNo);
        }));
    }
    
    for (var i = 1; i <= scopedServiceCTasksCount; i++)
    {
        // capture value of i locally
        var currentInstanceNo = i;
        
        // create a service scope
        var scope = hostInner.Services.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        
        scopedTasks.Add(Task.Run(async () =>
        {
            // resolve the scoped service B through DI
            var scopedServiceC = serviceProvider.GetRequiredService<IMyScopedServiceC>();
            await scopedServiceC.AddToCache(transactionIds[scopedServiceBTasksCount + currentInstanceNo-1], currentInstanceNo, currentInstanceNo);
        }));
    }
    
    Console.WriteLine($"Program: Starting {scopedTasks.Count} Scoped Tasks!\n");
    
    await Task.WhenAll(scopedTasks);
    
    Console.WriteLine();
}

async Task RunSingletonSimulationStaticConcurrency(IHost hostInner)
{
    var singletonTasks = new List<Task>();

    for (var j = 1; j <= 1 ; j++)
    {
        // capture value of j locally
        var currentInstanceNo = j;
        
        // create a service scope
        var scope = hostInner.Services.CreateScope();
        var serviceProvider = scope.ServiceProvider;

        // do singleton work 
        singletonTasks.Add(Task.Run(async () =>
        {
            // resolve the singleton service through DI
            var singletonService = serviceProvider.GetRequiredService<IMySingletonService>();
            await singletonService.PrintTransactionsCacheAndDatabase(currentInstanceNo);
        }));
    }
    
    // for (var j = 10; j <= 10 ; j++)
    // {
    //     // capture value of j locally
    //     var currentInstanceNo = j;
    //     
    //     // create a service scope
    //     var scope = hostInner.Services.CreateScope();
    //     var serviceProvider = scope.ServiceProvider;
    //
    //     // do singleton work 
    //     singletonTasks.Add(Task.Run(async () =>
    //     {
    //         // resolve the singleton service through DI
    //         var singletonService = serviceProvider.GetRequiredService<IMySingletonService>();
    //         await singletonService.DoSingletonWork(currentInstanceNo);
    //     }));
    // }
    
    
    Console.WriteLine($"Program: Starting {singletonTasks.Count} Singleton Tasks!\n");
    await Task.WhenAll(singletonTasks);
    
    Console.WriteLine();
    Console.WriteLine($"Program: - CounterScopedServiceA: {MyScopedServiceA.CounterScopedServiceA}");
    Console.WriteLine($"Program: - CounterScopedServiceB: {MyScopedServiceB.CounterScopedServiceB}");
    Console.WriteLine($"Program: - CounterScopedServiceB: {MyScopedServiceC.CounterScopedServiceC}");
    Console.WriteLine($"Program: - CounterSingletonService: {MySingletonService.CounterSingletonService}");
}