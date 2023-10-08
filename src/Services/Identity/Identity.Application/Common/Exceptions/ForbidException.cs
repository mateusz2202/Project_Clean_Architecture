namespace Identity.Application.Common.Exceptions;

public class ForbidException : Exception
{
    public ForbidException(string? message) : base(message)
    {

    }
}
