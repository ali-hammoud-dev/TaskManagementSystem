using System.Net;

namespace TaskManagementSystem.Common.Exceptions;

public class PlatformExceptionBuilder
{
    #region Private Fields
    private HttpStatusCode _statusCode;
    private string _errorMessage;
    #endregion

    #region Public Methods
    public PlatformExceptionBuilder StatusCode(HttpStatusCode statusCode)
    {
        _statusCode = statusCode;
        return this;
    }

    public PlatformExceptionBuilder ErrorMessage(string errorMessage)
    {
        _errorMessage = errorMessage;
        return this;
    }

    public PlatformException Build() => new(_statusCode, _errorMessage);
    #endregion
}

