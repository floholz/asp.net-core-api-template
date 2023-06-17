using asp.net_core_api_template.Models.Enums;

namespace asp.net_core_api_template.Models.Responses;

public class UserResponse: IResponse
{
    public UserResponse(User user)
    {
        Id = user.Id;
        Email = user.Email;
        UserRole = user.UserRole;
        UserName = user.UserName;
        FirstName = user.FirstName;
        LastName = user.LastName;
        PhotoBase64 = user.PhotoBase64;
        CreatedAt = user.CreatedAt;
        CreatedById = user.CreatedById;
        UpdatedAt = user.UpdatedAt;
        UpdatedById = user.UpdatedById;
        IsDeleted = user.IsDeleted;
        DeletedAt = user.DeletedAt;
        DeleteById = user.DeleteById;
    }

    public int Id { get; set; }
    public string Email { get; set; }
    public Role UserRole { get; set; }
    public string? UserName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhotoBase64 { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? CreatedById { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int? UpdatedById { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public int? DeleteById { get; set; }
}