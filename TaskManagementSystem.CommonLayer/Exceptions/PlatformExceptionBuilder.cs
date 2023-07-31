using System.Net;

namespace TaskManagementSystem.Common.Exceptions;

public class PlatformExceptionBuilder
{
    #region Private Fields
    private HttpStatusCode _statusCode;
    private Exception _innerException;
    private string _errorMessage;
    #endregion

    #region Public Methods

    public PlatformExceptionBuilder InnerException(Exception innerException)
    {
        _innerException = innerException;
        return this;
    }

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

    public PlatformException Build() => new(_statusCode, _innerException, _errorMessage);
    #endregion
}

