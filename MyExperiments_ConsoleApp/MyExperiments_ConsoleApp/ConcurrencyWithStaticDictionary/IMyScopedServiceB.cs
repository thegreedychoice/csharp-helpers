namespace MyExperiments_ConsoleApp.ConcurrencyWithStaticDictionary;

public interface IMyScopedServiceB
{
    Task AddToCache(string key, int value, int number = default);
}