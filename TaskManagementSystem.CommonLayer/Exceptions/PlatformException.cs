namespace TaskManagementSystem.Common.Exceptions;

public class PlatformException : Exception
{
    #region Properties 
    public int StatusCode { get; }

    public string ErrorMessage { get; }
    #endregion

    #region Constructor
    internal PlatformException(int statusCode, string errorMessage)
    {
        StatusCode = statusCode;
        ErrorMessage = errorMessage;
    }
    #endregion
}

