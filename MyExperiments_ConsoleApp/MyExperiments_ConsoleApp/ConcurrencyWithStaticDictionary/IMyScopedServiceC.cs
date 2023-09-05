namespace MyExperiments_ConsoleApp.ConcurrencyWithStaticDictionary;

public interface IMyScopedServiceC
{
    Task AddToCache(string key, int value, int number = default);
}