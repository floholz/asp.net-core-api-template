using asp.net_core_api_template.Database;
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
}