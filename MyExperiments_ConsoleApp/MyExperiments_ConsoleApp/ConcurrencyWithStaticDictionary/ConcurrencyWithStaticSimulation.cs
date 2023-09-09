using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MyExperiments_ConsoleApp.ConcurrencyWithStaticDictionary;


/// <summary>
/// What are we testing with this? - Concurrency with static data structures
///
/// - The ScopedServiceA will have a static concurrent dictionary which acts like a cache for recording transactions being processed currently.
/// - The SingletonService will hold another static concurrent  dictionary that acts like a Db Table.
/// - The critical section within the scoped services processes a request that is unique and needs to ignore duplicate requests.
/// - Then we run a simulation of sending multiple duplicate requests to this scoped service method.
/// - Then we need to test and ensure that the database updates are only happening once for each transaction and
///     concurrent duplicate requests are being discarded or failed.
///
/// Findings from this experiment:
///     1. Static data structures inside a scoped service are shared across all instances of the same and other types of scoped services.
///     2. Don't use Reflection to create instances of the services injected via DI as it creates a new instance everytime for all types of services.
///     3. Capture the local counter variables that are being used inside multi-threading environments such as in this case counters like i, j are
///         being passed to Tasks.Run() for all tasks being created in a loop and capturing it first and passing that value would provide accurate values.
///     4. To create new tasks, use Task.Run() as it accepts Func<Task> vs Action inside Thread.Start()
///     5. Be careful using static counters inside services like scoped/transient (CounterScopedServiceA) as it doesn't always provide the most accurate count
///         when trying to print the results using Console.WriteLine()
/// 
/// </summary>
public static class ConcurrencyWithStaticSimulation
{
    public static async Task RunScopedSimulationStaticConcurrency(IHost hostInner)
    {
        var scopedTasks = new List<Task>();

        const int scopedServiceATasksCount = 70;
        const int scopedServiceBTasksCount = 40;
        const int scopedServiceCTasksCount = 30;

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

    public static async Task RunSingletonSimulationStaticConcurrency(IHost hostInner)
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
        Console.WriteLine($"Program: - CounterScopedServiceC: {MyScopedServiceC.CounterScopedServiceC}");
        Console.WriteLine($"Program: - CounterSingletonService: {MySingletonService.CounterSingletonService}");
    }
}