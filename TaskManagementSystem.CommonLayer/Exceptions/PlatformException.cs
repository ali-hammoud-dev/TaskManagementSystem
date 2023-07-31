using System.Net;

namespace TaskManagementSystem.Common.Exceptions;

public class PlatformException : Exception
{
    #region Properties 
    public HttpStatusCode StatusCode { get; }

    public Exception InnerException { get; }

    public string ErrorMessage { get; }
    #endregion

    #region Constructor
    internal PlatformException(HttpStatusCode statusCode, Exception innerException, string errorMessage)
    {
        StatusCode = statusCode;
        InnerException = innerException;
        ErrorMessage = errorMessage;
    }
    #endregion
}

