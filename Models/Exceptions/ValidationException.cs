namespace asp.net_core_api_template.Models.Exceptions;

public class ValidationException : Exception
{
    public ValidationException(string? message) : base(message) { }
}