namespace TaskManagementSystem.Logging.Interfaces;

public interface ILoggerService
{
    public void LogInfo(string message, bool include = true);
    public void LogErrorException(Exception ex);
}

