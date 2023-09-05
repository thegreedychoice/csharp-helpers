namespace MyExperiments_ConsoleApp.ConcurrencyWithStaticDictionary;

public interface IMyScopedServiceA
{
    Task AddToCache(string key, int value, int number = default);
}