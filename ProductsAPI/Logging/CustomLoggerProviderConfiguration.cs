namespace ProductsAPI.Logging;

public class CustomLoggerProviderConfiguration // CustomLoggerProviderConfiguration class to configure the custom logger provider
{
    // The minimum log level
    public LogLevel LogLevel { get; set; } = LogLevel.Warning;

    // The event id
    public int EventId { get; set; } = 0;
}
