namespace asp.net_core_api_template.Models.Exceptions;

public class RefreshTokenExpiredException : Exception
{
    public RefreshTokenExpiredException(string? message) : base(message) { }
}