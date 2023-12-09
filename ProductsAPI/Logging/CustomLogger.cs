using System.Collections.Concurrent;

namespace ProductsAPI.Logging;

public class CustomLogger : ILogger
{
    private readonly string _categoryName;
    private readonly ConcurrentDictionary<string, CustomLogger> _loggers;

    public CustomLogger(string categoryName, ConcurrentDictionary<string, CustomLogger> loggers)
    {
        _categoryName = categoryName;
        _loggers = loggers;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        throw new NotImplementedException();
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        throw new NotImplementedException();
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        throw new NotImplementedException();
    }

    public void LogInformation(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"{DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")} {_categoryName} - {message}");
        Console.ResetColor();
    }
}
