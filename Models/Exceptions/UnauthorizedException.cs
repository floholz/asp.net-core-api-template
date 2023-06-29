namespace asp.net_core_api_template.Models.Exceptions;

public class UnauthorizedException : Exception
{
    public UnauthorizedException(string? message) : base(message) { }
}