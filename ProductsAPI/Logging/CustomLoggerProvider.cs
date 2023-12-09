using System.Collections.Concurrent;

namespace ProductsAPI.Logging;

public class CustomLoggerProvider : ILoggerProvider
{
    private readonly CustomLoggerProviderConfiguration _config;

    public CustomLoggerProvider(CustomLoggerProviderConfiguration config)
    {
        _config = config;
    }

    private readonly ConcurrentDictionary<string, CustomLogger> _loggers = new ConcurrentDictionary<string, CustomLogger>();

    public ILogger CreateLogger(string categoryName)
    {
        return _loggers.GetOrAdd(categoryName, name => new CustomLogger(name, _loggers));
        
    }

    public void Dispose()
    {
        _loggers.Clear();
    }
}
