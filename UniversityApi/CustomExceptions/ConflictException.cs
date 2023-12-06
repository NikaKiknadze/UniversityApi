﻿namespace UniversityApi.CustomExceptions
{
    public class ConflictException : Exception
    {
        public ConflictException(string message) : base(message)
        {

        }

        public ConflictException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
