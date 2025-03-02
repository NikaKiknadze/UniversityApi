namespace University.Domain.CustomExceptions
{
    public abstract class NoContentException : Exception
    {
        protected NoContentException(string message) : base(message)
        {

        }

        protected NoContentException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
