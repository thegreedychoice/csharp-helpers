namespace MyExperiments_ConsoleApp.ConcurrencyWithStaticDictionary;

public class MyScopedServiceB: IMyScopedServiceB
{
    private readonly IMySingletonService _mySingletonService;

    private static string? _className;
    
    public static int CounterScopedServiceB = 0;
    public MyScopedServiceB(IMySingletonService mySingletonService)
    {
        _className = GetType().Name;
        
        _mySingletonService = mySingletonService;
        
        CounterScopedServiceB++;
        Console.WriteLine($"{_className}: Constructor - CounterScopedServiceB: {CounterScopedServiceB}\n");
    }
    
    public async Task AddToCache(string key, int value, int number = default)
    {
        var newKey = key;
        var isAddedToCache = MyScopedServiceA.TransactionsCacheScopedServiceA.TryAdd(newKey, value);

        if (isAddedToCache)
        {
            // if add to cache successful, make a db call
            Console.WriteLine($"{_className}_{number}: AddToCache() - Success: Added key: {newKey} to Cache - Cache Count (After): {MyScopedServiceA.TransactionsCacheScopedServiceA.Count}");

            await _mySingletonService.AddToTheDatabase(key, 200, number);
        }
        else
        {
            // if not able to add to cache, return as db already has the entry
            Console.WriteLine($"{_className}_{number}: AddToCache() - Fail: Key: {newKey} already present in Cache - Cache Count (After): {MyScopedServiceA.TransactionsCacheScopedServiceA.Count}");
            return;
        }
    }
}