namespace asp.net_core_api_template.Models.Exceptions;

public class InvalidCredentialsException : Exception
{
    public InvalidCredentialsException(string? message) : base(message) { }
}