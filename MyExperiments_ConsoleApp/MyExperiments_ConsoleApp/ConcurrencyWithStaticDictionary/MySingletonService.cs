using System.Collections.Concurrent;

namespace MyExperiments_ConsoleApp.ConcurrencyWithStaticDictionary;

public class MySingletonService : IMySingletonService
{
    private readonly ConcurrentDictionary<string, int> _requestDbTable;

    public static int CounterSingletonService = 0;
    
    public MySingletonService()
    {
        _requestDbTable = new();
        CounterSingletonService++;
        Console.WriteLine($"MySingletonService: Constructor - CounterSingletonService: {CounterSingletonService}");
        Console.WriteLine();
    }
    
    public async Task PrintTransactionsCacheAndDatabase(int number = default)
    {
        await Task.Delay(100);
        
        Console.WriteLine("\n#############################################################\n");
        
        // print the cache
        Console.WriteLine($"MySingletonService_{number}: PrintTransactionsCacheAndDatabase() - Cache Count: {MyScopedServiceA.TransactionsCacheScopedServiceA.Count}");
        foreach (var (k,v) in MyScopedServiceA.TransactionsCacheScopedServiceA)
        {
            Console.WriteLine($"MySingletonService_{number} => {k}: {v}");
        }
        
        // print the cache
        Console.WriteLine($"\nMySingletonService_{number}: PrintTransactionsCacheAndDatabase() - Db table Count: {_requestDbTable.Count}");
        foreach (var (k,v) in _requestDbTable)
        {
            Console.WriteLine($"MySingletonService_{number} => {k}: {v}");
        }
        
        
        Console.WriteLine("\n#############################################################\n");
    }

    public async Task AddToTheDatabase(string dbKey, int dbValue, int number = default)
    {
        await Task.Delay(100);
        
        // if row already present, check the value and only add if its false
        if(_requestDbTable.TryGetValue(dbKey, out var value))
        {
            Console.WriteLine($"MySingletonService_{number}: AddToTheDatabase() - Fail: DbRecord for key: {dbKey} and value: {value} already present in db table");
        }
        else
        {
            _requestDbTable.TryAdd(dbKey, dbValue);
            Console.WriteLine($"MySingletonService_{number}: AddToTheDatabase() - Success: Added DbRecord with key: {dbKey} and value:  {dbValue} to db table");
        }
        
        // Add to the Singleton Db
        Console.WriteLine($"MySingletonService_{number}: AddToTheDatabase() - Database Table Count: {_requestDbTable.Count}"); }
}