using Microsoft.Extensions.Logging;
using SplitSampleApp.Test;

using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder
        .AddFilter("Microsoft", LogLevel.Warning)
        .AddFilter("System", LogLevel.Warning)
        .AddFilter("LoggingConsoleApp.Program", LogLevel.Debug)
        .AddConsole();
});
ILogger logger = loggerFactory.CreateLogger<Program>();

logger.LogInformation("Waiting for the initialization of the sample app.");
await Task.Delay(30000);

logger.LogInformation("Test begins.");
var httpClient = new HttpClient();

for (int i = 0; i < 10000; i++)
{
    var testClient = new TestClient(logger, httpClient);
    try
    {
        await testClient.Run();
    }
    catch (Exception e)
    {
        logger.LogError(e, "Encountered an unexpected error: {errorMessage}", e.Message);
    }

    await Task.Delay(100);
}

logger.LogInformation("Test ends.");
