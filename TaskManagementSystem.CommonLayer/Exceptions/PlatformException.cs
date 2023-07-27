using System.Net;

namespace TaskManagementSystem.Common.Exceptions;

public class PlatformException : Exception
{
    #region Properties 
    public HttpStatusCode StatusCode { get; }

    public string ErrorMessage { get; }
    #endregion

    #region Constructor
    internal PlatformException(HttpStatusCode statusCode, string errorMessage)
    {
        StatusCode = statusCode;
        ErrorMessage = errorMessage;
    }
    #endregion
}

