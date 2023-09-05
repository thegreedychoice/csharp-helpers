namespace MyExperiments_ConsoleApp.ConcurrencyWithStaticDictionary;

public interface IMySingletonService
{
    Task PrintTransactionsCacheAndDatabase(int number = default);

    Task AddToTheDatabase(string dbKey, int dbValue, int number = default);
}