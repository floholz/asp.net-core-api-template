using asp.net_core_api_template.Database;
using asp.net_core_api_template.Models;
using asp.net_core_api_template.Models.Exceptions;
using asp.net_core_api_template.Models.Responses;
using Microsoft.EntityFrameworkCore;

namespace asp.net_core_api_template.Services;

public class UserService
{
    private readonly PostgresContext _postgresContext;
        
    private readonly ILogger<UserService> _logger;

    public UserService(ILogger<UserService> logger, PostgresContext context)
    {
        _logger = logger;
        _postgresContext = context;
    }

    public ICollection<UserResponse> GetUsers()
    {
        return _postgresContext.Users
            .Include(user => user.UserRole)
            .Select(user => user.Map())
            .ToList();
    }

    /// <summary>
    /// Checks if user with this email and password exists and the password is correct
    /// Throws UserNotFoundException when user with email is not found in db
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    protected internal async Task<User?> ValidateUser(string email, string password)
    {
        // if user with this email and password exists, user is valid
        var validUser = await _postgresContext.Users
            .SingleOrDefaultAsync(e => email.Equals(e.Email));
        if (validUser == null)
        {
            throw new NotFoundException("User with this email not in DB");
        }

        return BCrypt.Net.BCrypt.Verify(password, validUser.Password) ? validUser : null;
    }
}