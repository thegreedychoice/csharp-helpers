using System.Collections.Concurrent;

namespace MyExperiments_ConsoleApp.ConcurrencyWithStaticDictionary;

public class MyScopedServiceA: IMyScopedServiceA
{
    private readonly IMySingletonService _mySingletonService; // singleton capture inside scoped service is fine

    private static string? _className;

    public static volatile int CounterScopedServiceA = 0;
    
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
            Console.WriteLine(
                $"{_className}_{number}: AddToCache() - Success: Added key: {newKey} to Cache - Cache Count (After): {TransactionsCacheScopedServiceA.Count}");

            await _mySingletonService.AddToTheDatabase(key, 1000, number);
            
            // remove from cache after successfully added to database
            if (MyScopedServiceA.TransactionsCacheScopedServiceA.TryRemove(newKey, out var isRemovedFromCache))
            {
                Console.WriteLine(
                    $"{_className}_{number}: AddToCache() - Success: Removed key: {newKey} from Cache - Cache Count (After): {MyScopedServiceA.TransactionsCacheScopedServiceA.Count}");
            }
            else
            {
                Console.WriteLine(
                    $"{_className}_{number}: AddToCache() - Fail: Unable to Remove key: {newKey} from Cache - Cache Count (After): {MyScopedServiceA.TransactionsCacheScopedServiceA.Count}");
            }
        }
        else
        {
            // if not able to add to cache, return as db already has the entry
            Console.WriteLine(
                $"{_className}_{number}: AddToCache() - Fail: Key: {newKey} already present in Cache - Cache Count (After): {TransactionsCacheScopedServiceA.Count}");
        }
    }
}