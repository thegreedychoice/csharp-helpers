using System.Collections.Concurrent;

namespace MyExperiments_ConsoleApp.ConcurrencyWithStaticDictionary;

/// <summary>
/// What are we testing with this? - Concurrency with static data structures
///
/// - This scoped service will have a static dictionary and a semaphore that controls concurrent threads.
/// how many threads can access a critical section.
/// - The critical section processes a request that is unique and needs to ignore duplicate requests.
/// - We have a singleton dictionary called `RequestDbTable` that records all the requestIds coming in.
///     - If a request is not present in this table, it is allowed inside the critical section.
/// - Then we run a simulation of sending multiple duplicate requests to this scoped service method.
/// - Then we need to test and ensure that the critical section was only invoked once. 
/// </summary>
public class MyScopedServiceA: IMyScopedServiceA
{
    private readonly IMySingletonService _mySingletonService; // singleton capture inside scoped service is fine

    private static string? _className;

    public static int CounterScopedServiceA = 0;
    
    public static readonly ConcurrentDictionary<string, int> TransactionsCacheScopedServiceA = new ();
    
    public MyScopedServiceA(IMySingletonService mySingletonService)
    {
        _className = GetType().Name;
        
        _mySingletonService = mySingletonService;
        CounterScopedServiceA++;
        Console.WriteLine($"{_className}: Constructor - CounterScopedServiceA: {CounterScopedServiceA}\n");
    }
    
    public async Task AddToCache(string key, int value, int number = default)
    {
        var newKey = key;
        var isAddedToCache = MyScopedServiceA.TransactionsCacheScopedServiceA.TryAdd(newKey, value);

        if (isAddedToCache)
        {
            // if add to cache successful, make a db call
            Console.WriteLine($"{_className}_{number}: AddToCache() - Success: Added key: {newKey} to Cache - Cache Count (After): {TransactionsCacheScopedServiceA.Count}");

            await _mySingletonService.AddToTheDatabase(key, 100, number);
        }
        else
        {
            // if not able to add to cache, return as db already has the entry
            Console.WriteLine($"{_className}_{number}: AddToCache() - Fail: Key: {newKey} already present in Cache - Cache Count (After): {TransactionsCacheScopedServiceA.Count}");
            return;
        }
    }
}