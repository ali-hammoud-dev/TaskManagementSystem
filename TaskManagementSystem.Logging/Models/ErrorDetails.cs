namespace TaskManagementSystem.Logging.Models;

public class ErrorDetails
{
    public string Source { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public string StackTrace { get; set; } = string.Empty;

    public string InnerExceptionMessage { get; set; } = string.Empty;

    public string InnerExceptionStackTrace { get; set; } = string.Empty;
}