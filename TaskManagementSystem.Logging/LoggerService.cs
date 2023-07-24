using Microsoft.AspNetCore.Http;
using NLog;
using TaskManagementSystem.Logging.Interfaces;
using TaskManagementSystem.Logging.Models;

namespace TaskManagementSystem.Logging;

public class LoggerService : ILoggerService
{
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
    private readonly IHttpContextAccessor _contextAccessor;

    public LoggerService(IHttpContextAccessor accessor) => _contextAccessor = accessor;

    public void LogInfo(string message, bool includeDetails = true)
    {
        if (includeDetails) ScopeContext.PushProperties(GetCommonLogProperties());
        Logger.Info(message);
    }

    public void LogErrorException(Exception ex)
    {
        throw new NotImplementedException();
    }

    #region Private Methods

    private List<KeyValuePair<string, object>> GetCommonLogProperties()
    {
        var commonDetails = GetCommonLogDetails() ?? new();

        var logProperties = new List<KeyValuePair<string, object>>()
        {
            new KeyValuePair<string, object>("RequestPath", commonDetails.RequestPath),
            new KeyValuePair<string, object>("RequestQueryString", commonDetails.RequestQueryString),
        };

        return logProperties;
    }


    private CommonLogDetails GetCommonLogDetails()
    {
        var requestInfo = _contextAccessor.HttpContext?.Request;

        return new CommonLogDetails
        {
            RequestPath = requestInfo?.Path ?? "",
            RequestQueryString = requestInfo?.QueryString.ToString() ?? "",
        };
    }

    private ErrorDetails GetErrorDetails(Exception ex)
    {
        return new ErrorDetails
        {
            Source = ex.Source ?? "",
            Message = ex.Message,
            StackTrace = ex.StackTrace ?? "",
            InnerExceptionMessage = ex.InnerException?.Message ?? "",
            InnerExceptionStackTrace = ex.InnerException?.StackTrace ?? "",
        };
    }

    private List<KeyValuePair<string, object>> GetErrorLogProperties(ErrorDetails errorDetails)
    {
        var logProperties = new List<KeyValuePair<string, object>>()
        {
            new KeyValuePair<string, object>("Source", errorDetails.Source),
            new KeyValuePair<string, object>("StackTrace", errorDetails.StackTrace),
            new KeyValuePair<string, object>("InnerExceptionMessage", errorDetails.InnerExceptionMessage),
            new KeyValuePair<string, object>("InnerExceptionStackTrace", errorDetails.InnerExceptionStackTrace)
        };

        return logProperties;
    }

    private void LogError(ErrorDetails errorDetails)
    {
        var logProperties = GetCommonLogProperties().Concat(GetErrorLogProperties(errorDetails)).ToList();
        ScopeContext.PushProperties(logProperties);
        Logger.Error(errorDetails.Message);
    }
    #endregion

}

