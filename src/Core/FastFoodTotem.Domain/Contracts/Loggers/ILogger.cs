namespace FastFoodTotem.Domain.Contracts.Loggers;

public interface ILogger
{
    Task Log(string stackTrace, string message, string exception);
}
