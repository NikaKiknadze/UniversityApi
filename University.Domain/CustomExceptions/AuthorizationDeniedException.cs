namespace University.Domain.CustomExceptions;

public class AuthorizationDeniedException : Exception
{
    public AuthorizationDeniedException(string message) : base(message)
    {

    }

    public AuthorizationDeniedException(string message, Exception innerException) : base(message, innerException)
    {

    }
}